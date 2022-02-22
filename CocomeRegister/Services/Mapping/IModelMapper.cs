using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Mapping
{
    public interface IModelMapper
    {
        Product CreateProduct(ProductTO productTO);
        Order CreateOrder(OrderTO orderTO);
        OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO);
        SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO);

        ProductTO CreateProductTO(Product product);
        OrderElementTO CreateOrderElementTO(OrderElement orderElement);
        OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements);

        void UpdateProduct(Product product, ProductTO productTO);
    }
}
