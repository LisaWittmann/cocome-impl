using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Database;
using CocomeStore.Models.Transfer;
using CocomeStore.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CocomeStore.Services
{
    /// <summary>
    /// class <c>ExchangeService</c> implements <see cref="IExchangeService"/>
    /// and provides functionalities to perform stock exchanges between stores
    /// </summary>
    public class ExchangeService : IExchangeService
    {
        private readonly ILogger<ExchangeService> _logger;
        private readonly IModelMapper _mapper;
        private readonly CocomeDbContext _context;

        public ExchangeService(
            ILogger<ExchangeService> logger,
            IModelMapper mapper,
            CocomeDbContext context
        )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// method <c>GetIncommingProducts</c> filters the product of a store
        /// that are either ordered or in a stock exchange with another store
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>list of incomming products</returns>
        private IEnumerable<Product> GetIncommingProducts(int storeId)
        {
            var incommingProducts = _context.ExchangeElements
                .Include(element => element.StockExchange)
                .Where(element =>
                    element.StockExchange.DeliveringDate <= element.StockExchange.PlacingDate &&
                    element.StockExchange.StoreId == storeId)
                .Select(element => element.Product)
                .ToList();
            incommingProducts.AddRange(_context.OrderElements
                .Include(element => element.Order)
                .Where(element =>
                    element.Order.DeliveringDate < element.Order.PlacingDate &&
                    element.Order.StoreId == storeId)
                .Select(element => element.Product));
            return incommingProducts.ToHashSet();
        }

        /// <summary>
        /// method <c>GetLowStockItems</c> filters the stock items of a store
        /// that are running out of stock but are not included in an incomming
        /// order or a currently performed stock exchange
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>list of stock items of the store</returns>
        public IEnumerable<StockItem> GetLowStockItems(int storeId)
        {
            var incommingItems = GetIncommingProducts(storeId)
                .Select(product => product.Id);
            return _context.StockItems
                .Where(item =>
                    item.StoreId == storeId &
                    item.Minimum != 0 &&
                    item.Stock <= item.Minimum)
                .AsEnumerable()
                .Where(item => !incommingItems.Contains(item.ProductId));
        }

        /// <summary>
        /// method <c>GetNearStoresAsync</c> filters the nearby stores to the
        /// given store - a store is recognized as nearby if its postalcode starts
        /// with the same number as the given store
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>list of stores that are near enough for an exchange</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        private async Task<IEnumerable<int>> GetNearStoresAsync(int storeId)
        {
            var store = await _context.Stores.FindAsync(storeId);
            if (store == null)
            {
                throw new EntityNotFoundException();
            }

            if (store.PostalCode == 0)
            {
                return new List<int>();
            }

            var locationParam = store.PostalCode.ToString().Substring(0, 2);
            return _context.Stores
                .Where(store => store.PostalCode != 0)
                .AsEnumerable()
                .Where(store =>
                    store.PostalCode
                        .ToString()
                        .StartsWith(locationParam))
                .Select(store => store.Id);
        }

        /// <summary>
        /// method <c>GetExchangingStoreAsync</c> chooses the store of the nearby
        /// stores, that has at least the tripled stock of the required minimum
        /// stock for the product and selects the store with the highest stock
        /// from all possible exchange providers
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <param name="productId">unique identifier of the product to exchange</param>
        /// <returns>best provider for the given product and store</returns>
        /// <exception cref="EntityNotFoundException"></exception>
        private async Task<Store> GetExchangingStoreAsync(int storeId, int productId)
        {
            var nearStores = await GetNearStoresAsync(storeId);

            if (nearStores.ToArray().Length == 0)
            {
                return null;
            }

            var stocks = _context.StockItems
                .Where(item =>
                    item.ProductId == productId &&
                    item.Stock > item.Minimum * 4)
                .AsEnumerable()
                .Where(item => nearStores.Contains(item.StoreId))
                .ToArray();

            if (stocks.Length == 0)
            {
                return null;
            }

            var max = stocks.Max(item => item.Stock);
            return stocks
                .Where(item => item.Stock == max)
                .Select(item => item.Store)
                .SingleOrDefault();
        }

        /// <summary>
        /// method <c>CheckForExchangesAsync</c> searches for products that are
        /// running out of stock in the given store and calculates if any other
        /// store nearby has the product in stock and creates a new stock excange
        /// if a possible providing store was found
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task CheckForExchangesAsync(int storeId)
        {
            var lowStockItems = GetLowStockItems(storeId).ToArray();
            var exchanges = new List<StockExchange>();

            foreach (var item in lowStockItems)
            {
                var sendingStore = await GetExchangingStoreAsync(storeId, item.ProductId);
                if (sendingStore == null)
                {
                    _logger.LogInformation("No sending store found for product {}", item.ProductId);
                    continue;
                }

                var exchange = exchanges
                    .Where(ex => ex.ProviderId == sendingStore.Id)
                    .SingleOrDefault();
                var amount = item.Minimum * 2;

                if (exchange == null)
                {
                    exchange = new StockExchange()
                    {
                        ProviderId = sendingStore.Id,
                        StoreId = storeId,
                        PlacingDate = DateTime.MinValue,
                        DeliveringDate = DateTime.MinValue,
                    };
                }

                _context.ExchangeElements.Add(new ()
                {
                    ProductId = item.ProductId,
                    Amount = amount,
                    StockExchange = exchange,
                });
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// method <c>GetStockExchanges</c> filters all open stock exchanges where
        /// the given store is either the provider or the receiving store
        /// </summary>
        /// <param name="storeId">unique identifier of the store</param>
        /// <returns>all open exchanges related to the store</returns>
        public IEnumerable<StockExchangeTO> GetStockExchanges(int storeId)
        {
            return _context.StockExchanges
                .Where(exchange =>
                    (exchange.StoreId == storeId &&
                    exchange.DeliveringDate != exchange.PlacingDate &&
                    exchange.DeliveringDate < exchange.PlacingDate) ||
                    (exchange.ProviderId == storeId &&
                    exchange.PlacingDate <= exchange.DeliveringDate))
                .Include(exchange => exchange.Provider)
                .Include(exchange => exchange.Store)
                .AsEnumerable()
                .GroupJoin(_context.ExchangeElements
                    .Include(element => element.Product),
                    exchange => exchange.Id,
                    element => element.StockExchangeId,
                    (exchange, elements) => _mapper.CreateStockExchangeTO(
                        exchange, elements.AsEnumerable()));
        }

        /// <summary>
        /// method <c>CloseStockExchange</c>
        /// </summary>
        /// <param name="exchange"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        public void CloseStockExchange(StockExchangeTO exchange)
        {
            var stockExchange = _context.StockExchanges.Find(exchange.Id);
            if (stockExchange == null)
            {
                throw new EntityNotFoundException();
            }

            foreach (var element in exchange.Elements)
            {
                var stockItem = _context.StockItems
                    .Where(item =>
                        item.StoreId == stockExchange.StoreId &&
                        item.ProductId == element.Product.Id)
                    .SingleOrDefault();
                stockItem.Stock += element.Amount;
            }
            stockExchange.DeliveringDate = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// method <c>StartStockExchange</c>
        /// </summary>
        /// <param name="exchange"></param>
        /// <exception cref="EntityNotFoundException"></exception>
        public void StartStockExchange(StockExchangeTO exchange)
        {
            var stockExchange = _context.StockExchanges.Find(exchange.Id);
            if (stockExchange == null)
            {
                throw new EntityNotFoundException();
            }

            foreach (var element in exchange.Elements)
            {
                var stockItem = _context.StockItems
                    .Where(item =>
                        item.StoreId == stockExchange.ProviderId &&
                        item.ProductId == element.Product.Id)
                    .SingleOrDefault();
                stockItem.Stock -= element.Amount;
            }
            stockExchange.PlacingDate = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
