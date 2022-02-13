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

        IEnumerable<StockItem> GetInventory(int storeId);
        void CreateProduct(int storeId, ProductTO productTO);
        void UpdateProduct(int storeId, ProductTO productTO);
        void UpdateStock(int storeId, int productId, int stock);

        float GetProfitOfMonth(int storeId, int month, int year);
    }
}
