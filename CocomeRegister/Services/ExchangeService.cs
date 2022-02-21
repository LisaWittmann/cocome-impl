using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocomeStore.Exceptions;
using CocomeStore.Models;
using CocomeStore.Models.Database;
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
        private readonly CocomeDbContext _context;

        public ExchangeService(
            ILogger<ExchangeService> logger,
            CocomeDbContext context
        )
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public IEnumerable<Product> GetIncommingProducts(int storeId)
        {
            var incommingProducts = _context.ExchangeElements
                .Include(element => element.StockExchange)
                .Where(element =>
                    element.StockExchange.DeliveringDate != DateTime.MinValue &&
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
            _logger.LogInformation("get low stock items called");
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
        public async Task<IEnumerable<int>> GetNearStores(int storeId)
        {
            _logger.LogInformation("check for exchanges called");
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
            _logger.LogInformation("location param {}", locationParam);
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
        public async Task<Store> GetExchangingStore(int storeId, int productId)
        {
            _logger.LogInformation("get exchanging store called");
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
            _logger.LogInformation("check for exchanges called");
            var lowStockItems = GetLowStockItems(storeId).ToArray();
            var exchanges = new List<StockExchange>();
            var exchangeElements = new List<ExchangeElement>();

            if (lowStockItems.Length == 0)
            {
                return;
            }

            _logger.LogInformation("low stock items: {}", lowStockItems.Length);

            foreach (var item in lowStockItems)
            {
                var sendingStore = await GetExchangingStore(storeId, item.ProductId);
                if (sendingStore == null)
                {
                    _logger.LogInformation("No sending store found for product {}", item.ProductId);
                    continue;
                }

                _logger.LogInformation("sending store: {}", sendingStore.Id);

                var exchange = exchanges
                    .Where(ex => ex.SendingStoreId == sendingStore.Id)
                    .SingleOrDefault();
                var amount = item.Minimum + 1;

                if (exchange != null)
                {
                    exchange = new StockExchange()
                    {
                        SendingStoreId = sendingStore.Id,
                        ReceivingStoreId = storeId,
                        PlacingDate = DateTime.Now,
                        DeliveringDate = DateTime.MinValue,
                    };
                }
                exchangeElements.Add(new ()
                {
                    ProductId = item.ProductId,
                    Amount = amount,
                    StockExchange = exchange,
                });
                _logger.LogInformation("prodct id: {}", item.ProductId);
                var items =_context.StockItems
                    .Where(item =>
                        (item.StoreId == sendingStore.Id && item.ProductId == item.ProductId));
                _logger.LogInformation("stock items: {}", items.ToArray().Length);
            }
            await _context.ExchangeElements.AddRangeAsync(exchangeElements);
            await _context.SaveChangesAsync();
        }
    }
}
