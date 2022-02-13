using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services
{
    public class ModelMapper : IModelMapper
    {
        public ModelMapper()
        {
        }

        public OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO)
        {
            OrderElement orderElement = new()
            {
                ProductId = orderElementTO.Product.Id,
                Amount = orderElementTO.Amount,
                Order = order,
            };
            return orderElement;
        }

        public OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements)
        {
            var elements = orderElements;
            if (elements.Count() == 0)
            {
                elements = Array.Empty<OrderElement>();
            }
            OrderTO orderTO = new()
            {
                Id = order.Id,
                OrderElements = elements.ToArray(),
                Store = order.Store,
                Provider = order.Provider,
                Closed = order.Closed,
                Delivered = order.Delivered,
                PlacingDate = order.PlacingDate,
                DeliveringDate = order.DeliveringDate,
            };
            return orderTO;
        }

        public Product CreateProduct(ProductTO productTO)
        {
            Product product = new()
            {
                Name = productTO.Name,
                Price = productTO.Price,
                SalePrice = productTO.SalePrice,
                ProviderId = productTO.Provider.Id,
                Description = productTO.Description,
                ImageUrl = productTO.ImageUrl

            };
            return product;
        }

        public SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO)
        {
            SaleElement saleElement = new()
            {
                ProductId = saleElementTO.Product.Id,
                Amount = saleElementTO.Amount,
                Sale = sale
            };
            return saleElement;
        }

        public StockItem CreateStockItem(StockItemTO stockItemTO)
        {
            StockItem stockItem = new()
            {
                ProductId = stockItemTO.Product.Id,
                Stock = stockItemTO.Stock,
                StoreId = stockItemTO.Store.Id,
                Minimum = stockItemTO.Minimum
            };
            return stockItem;
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
