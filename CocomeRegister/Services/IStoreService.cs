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
        void PlaceOrder(int storeId, IEnumerable<OrderElement>);

        IEnumerable<StockItem> GetInventory(int storeId);
        void CreateProduct(int storeId, Product product);
        void UpdateStock(int storeId, int productId, int stock);
    }
}
