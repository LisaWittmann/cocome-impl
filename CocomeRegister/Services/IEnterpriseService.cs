using System;
using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IEnterpriseService
    {
        IEnumerable<Store> GetAllStores();
        IEnumerable<OrderTO> GetAllOrders();
        IEnumerable<StockItem> GetAllStockItems();
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Provider> GetAllProvider();

        void CreateProduct(ProductTO productTO);
        void UpdateProduct(int productId, ProductTO productTO);

        void CreateStore(Store storeTO);
        void UpdateStore(int storeId, Store storeTO);

        void CreateProvider(Provider providerTO);
        void UpdateProvider(int providerId, Provider providerTO);

        IEnumerable<TimeSpan> GetDeliverySpans(int providerId);
    }
}
