using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IStoreService
    {
        Store GetStore(int storeId);
        IEnumerable<Store> GetAllStores();

        IEnumerable<OrderTO> GetOrders(int storeId);
        void CloseOrder(int storeId, int orderId);
        void PlaceOrder(int storeId, IEnumerable<OrderElementTO> elements);

        ProductTO GetProduct(int storeId, int productId);
        void UpdateProduct(int storeId, ProductTO productTO);

        IEnumerable<StockItem> GetInventory(int storeId);
        void UpdateStock(int storeId, int productId, int stock);

        double GetProfitOfMonth(int storeId, int month, int year);
        Statistic GetProfitOfYear(int storeId, int year);
        IEnumerable<Statistic> GetProfit(int storeId);
    }
}
