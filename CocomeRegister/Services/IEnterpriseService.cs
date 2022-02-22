using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IEnterpriseService
    {
        IEnumerable<Store> GetAllStores();
        IEnumerable<OrderTO> GetAllOrders();
        IEnumerable<ProductTO> GetAllProducts();
        IEnumerable<Provider> GetAllProvider();

        ProductTO CreateProduct(ProductTO productTO);
        ProductTO UpdateProduct(int productId, ProductTO productTO);

        Store CreateStore(Store storeTO);
        Store UpdateStore(int storeId, Store storeTO);

        Provider CreateProvider(Provider providerTO);
        Provider UpdateProvider(int providerId, Provider providerTO);

        IEnumerable<Store> GetStores(int productId);
        void AddToStock(int storeId, int productId);
    }
}
