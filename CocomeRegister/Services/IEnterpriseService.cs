using System;
using System.Collections;
using System.Collections.Generic;
using CocomeStore.Models;

namespace CocomeStore.Services
{
    public interface IEnterpriseService
    {
        IEnumerable<Store> GetAllStores();
        IEnumerable<Order> GetAllOrders();
        IEnumerable<StockItem> GetAllStockItems();
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Provider> GetAllProvider();

        void CreateProduct(Product product);
        void UpdateProduct(int productId, Product product);

        void CreateStore(Store store);
        void UpdateStore(int storeId, Store store);

        void CreateProvider(Provider provider);
        void UpdateProvider(int providerId, Provider provider);

        IEnumerable<TimeSpan> GetDeliverySpans(int providerId);
    }
}
