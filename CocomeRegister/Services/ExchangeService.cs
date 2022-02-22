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
    /// 
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private IEnumerable<Product> GetIncommingProducts(int storeId)
        {
            var incommingProducts = _context.ExchangeElements
                .Include(element => element.StockExchange)
                .Where(element =>
                    element.StockExchange.DeliveringDate == DateTime.MinValue &&
                    element.StockExchange.ReceivingStoreId == storeId)
                .Select(element => element.Product)
                .ToList();
            incommingProducts.AddRange(_context.OrderElements
                .Include(element => element.Order)
                .Where(element => element.Order.StoreId == storeId)
                .Select(element => element.Product));
            return incommingProducts.ToHashSet();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetNearStores(int storeId)
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

            var locationParam = store.PostalCode.ToString().Substring(0, 1);
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<Store> GetExchangingStore(int storeId, int productId)
        {
            var nearStores = await GetNearStores(storeId);

            if (nearStores.ToArray().Length == 0)
            {
                return null;
            }

            var stocks = _context.StockItems
                .Where(item =>
                    item.ProductId == productId &&
                    item.Stock > item.Minimum * 3)
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public async Task CheckForExchanges(int storeId)
        {
            var lowStockItems = GetLowStockItems(storeId).ToArray();
            var exchanges = new List<StockExchange>();

            foreach (var item in lowStockItems)
            {
                var sendingStore = await GetExchangingStore(storeId, item.ProductId);
                if (sendingStore == null)
                {
                    _logger.LogInformation("No sending store found for product {}", item.ProductId);
                    continue;
                }

                var exchange = exchanges
                    .Where(ex => ex.SendingStoreId == sendingStore.Id)
                    .SingleOrDefault();
                var amount = item.Minimum + 1;

                if (exchange == null)
                {
                    exchange = new StockExchange()
                    {
                        SendingStoreId = sendingStore.Id,
                        ReceivingStoreId = storeId,
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
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public IEnumerable<StockExchangeTO> GetStockExchanges(int storeId)
        {
            return _context.StockExchanges
                .Where(exchange =>
                    (exchange.ReceivingStoreId == storeId &&
                    exchange.DeliveringDate == DateTime.MinValue) ||
                    (exchange.SendingStoreId == storeId &&
                    exchange.PlacingDate == DateTime.MinValue))
                .Include(exchange => exchange.SendingStore)
                .Include(exchange => exchange.ReceivingStore)
                .AsEnumerable()
                .GroupJoin(_context.ExchangeElements
                    .Include(element => element.Product),
                    exchange => exchange.Id,
                    element => element.StockExchangeId,
                    (exchange, elements) => _mapper.CreateStockExchangeTO(
                        exchange, elements.AsEnumerable()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
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
                        item.StoreId == stockExchange.ReceivingStoreId &&
                        item.ProductId == element.ProductId)
                    .SingleOrDefault();
                stockItem.Stock += element.Amount;
            }
            stockExchange.DeliveringDate = DateTime.Now;
            _context.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exchange"></param>
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
                        item.StoreId == stockExchange.SendingStoreId &&
                        item.ProductId == element.ProductId)
                    .SingleOrDefault();
                stockItem.Stock -= element.Amount;
            }
            stockExchange.PlacingDate = DateTime.Now;
            _context.SaveChanges();
        }
    }
}
