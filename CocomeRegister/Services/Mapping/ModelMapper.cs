using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Mapping
{
    public class ModelMapper : IModelMapper
    {
        public OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO)
        {
            return new ()
            {
                ProductId = orderElementTO.Product.Id,
                Amount = orderElementTO.Amount,
                Order = order,
            };
        }

        public OrderElementTO CreateOrderElementTO(OrderElement orderElement)
        {
            return new ()
            {
                Product = orderElement.Product,
                Amount = orderElement.Amount,
            };
        }

        public Order CreateOrder(OrderTO orderTO)
        {
            return new ()
            {
                StoreId = orderTO.Store.Id,
                ProviderId = orderTO.Provider.Id,
                PlacingDate = orderTO.PlacingDate,
            };
        }

        public OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements)
        {
            var elements = orderElements
                 .DefaultIfEmpty()
                 .Select(element => CreateOrderElementTO(element))
                 .ToArray();

            return new ()
            {
                Id = order.Id,
                OrderElements = elements,
                Store = order.Store,
                Provider = order.Provider,
                PlacingDate = order.PlacingDate,
                DeliveringDate = order.DeliveringDate,
                Closed = (order.DeliveringDate != DateTime.MinValue)
            };
        }

        public Product CreateProduct(ProductTO productTO)
        {
            return new ()
            {
                Name = productTO.Name,
                Price = productTO.Price,
                SalePrice = productTO.SalePrice,
                ProviderId = productTO.Provider.Id,
                Description = productTO.Description,
                ImageUrl = productTO.ImageUrl

            };
        }

        public ProductTO CreateProductTO(Product product)
        {
            return new ()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Provider = product.Provider
            };
        }

        public SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO)
        {
            return new ()
            {
                ProductId = saleElementTO.Product.Id,
                Amount = saleElementTO.Amount,
                Sale = sale
            };
        }

        public StockItem CreateStockItem(StockItemTO stockItemTO)
        {
            return new ()
            {
                ProductId = stockItemTO.Product.Id,
                Stock = stockItemTO.Stock,
                StoreId = stockItemTO.Store.Id,
                Minimum = stockItemTO.Minimum
            };
        }

        public StockExchangeTO CreateStockExchangeTO(
           StockExchange stockExchange,
           IEnumerable<ExchangeElement> exchangeElements
       )
        {
            return new()
            {
                Id = stockExchange.Id,
                SendingStore = stockExchange.SendingStore,
                ReceivingStore = stockExchange.ReceivingStore,
                ExchangeElements = exchangeElements.ToArray(),
                PlacingDate = stockExchange.PlacingDate,
                DeliveringDate = stockExchange.DeliveringDate,
                Closed = stockExchange.DeliveringDate != DateTime.MinValue
            };
        }

        public void UpdateProduct(Product product, ProductTO productTO)
        {
            product.Name = productTO.Name;
            product.Price = productTO.Price;
            product.SalePrice = productTO.SalePrice;
            product.Description = productTO.Description;
            product.ImageUrl = productTO.ImageUrl;
            product.ProviderId = productTO.Provider.Id;
        }

        public void UpdateStockItem(StockItem stockItem, StockItemTO stockItemTO)
        {
            stockItem.Stock = stockItemTO.Stock;
            stockItem.Minimum = stockItem.Minimum;
        }
    }
}
