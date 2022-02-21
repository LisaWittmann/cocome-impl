using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CocomeStore.Models;

namespace CocomeStore.Services
{
    public interface IExchangeService
    {
        IEnumerable<Product> GetIncommingProducts(int storeId);
        IEnumerable<StockItem> GetLowStockItems(int storeId);
        Task CheckForExchanges(int storeId);
    }
}
