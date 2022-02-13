using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public interface IModelMapper
    {
        Product CreateProduct(ProductTO productTO);
        OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO);
        SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO);
        StockItem CreateStockItem(StockItemTO stockItemTO);
        OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements);

        void UpdateProduct(Product product, ProductTO productTO);
        void UpdateStockItem(StockItem stockItem, StockItemTO stockItemTO);
    }
}
