using System;
using System.Collections.Generic;
using CocomeStore.Models;

namespace CocomeStore.Services
{
    public interface IStoreService
    {
        Store GetStore(int storeId);
        IEnumerable<Store> GetAllStores();

        IEnumerable<Order> GetOrders(int storeId);
        void CloseOrder(int storeId, int orderId);
        void PlaceOrder(int storeId, IEnumerable<OrderElement> elements);

        IEnumerable<StockItem> GetInventory(int storeId);
        void CreateProduct(int storeId, Product product);
        void UpdateProduct(int storeId, Product product);
        void UpdateStock(int storeId, int productId, int stock);

        float GetProfitOfMonth(int storeId, int month, int year);
        IEnumerable<float> GetProfitOfYear(int storeId, int year);
    }
}
