using System.Collections.Generic;
using CocomeStore.Models;
using CocomeStore.Models.Authorization;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Mapping
{
    public interface IModelMapper
    {
        Product CreateProduct(ProductTO productTO);
        Order CreateOrder(OrderTO orderTO);
        OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO);
        Sale CreateSale(SaleTO saleTO);
        SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO);
        ApplicationUser CreateApplicationUser(UserTO userTO);

        ProductTO CreateProductTO(Product product);
        OrderElementTO CreateOrderElementTO(OrderElement orderElement);
        OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements);
        ExchangeElementTO CreateExchangeElementTO(ExchangeElement exchangeElement);
        StockExchangeTO CreateStockExchangeTO(StockExchange stockExchange, IEnumerable<ExchangeElement> exchangeElements);

        void UpdateProduct(Product product, ProductTO productTO);
    }
}
