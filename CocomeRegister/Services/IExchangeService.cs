using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IExchangeService
    {
        IEnumerable<StockItem> GetLowStockItems(int storeId);
        IEnumerable<StockExchangeTO> GetStockExchanges(int storeId);
        Task CheckForExchangesAsync(int storeId);
        void CloseStockExchange(StockExchangeTO exchange);
        void StartStockExchange(StockExchangeTO exchange);
    }
}
