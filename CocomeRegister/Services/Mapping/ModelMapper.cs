using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelMapper : IModelMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderElementTO"></param>
        /// <returns></returns>
        public OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO)
        {
            OrderElement orderElement = new ()
            {
                ProductId = orderElementTO.Product.Id,
                Amount = orderElementTO.Amount,
                Order = order,
            };
            return orderElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderElement"></param>
        /// <returns></returns>
        public OrderElementTO CreateOrderElementTO(OrderElement orderElement)
        {
            OrderElementTO orderElementTO = new ()
            {
                Product = orderElement.Product,
                Amount = orderElement.Amount,
            };
            return orderElementTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderTO"></param>
        /// <returns></returns>
        public Order CreateOrder(OrderTO orderTO)
        {
            Order order = new()
            {
                StoreId = orderTO.Store.Id,
                ProviderId = orderTO.Provider.Id,
                PlacingDate = orderTO.PlacingDate,
            };
            return order;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="orderElements"></param>
        /// <returns></returns>
        public OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements)
        {
            var elements = orderElements
                 .DefaultIfEmpty()
                 .Select(element => CreateOrderElementTO(element))
                 .ToArray();

            OrderTO orderTO = new()
            {
                Id = order.Id,
                OrderElements = elements,
                Store = order.Store,
                Provider = order.Provider,
                PlacingDate = order.PlacingDate,
                DeliveringDate = order.DeliveringDate,
                Closed = (order.DeliveringDate != DateTime.MinValue)
            };
            return orderTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ProductTO CreateProductTO(Product product)
        {
            ProductTO productTO = new()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Provider = product.Provider
            };
            return productTO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="storeId"></param>
        /// <param name="saleElementTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockItemTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productTO"></param>
        public void UpdateProduct(Product product, ProductTO productTO)
        {
            product.Name = productTO.Name;
            product.Price = productTO.Price;
            product.SalePrice = productTO.SalePrice;
            product.Description = productTO.Description;
            product.ImageUrl = productTO.ImageUrl;
            product.ProviderId = productTO.Provider.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockItem"></param>
        /// <param name="stockItemTO"></param>
        public void UpdateStockItem(StockItem stockItem, StockItemTO stockItemTO)
        {
            stockItem.Stock = stockItemTO.Stock;
            stockItem.Minimum = stockItem.Minimum;
        }
    }
}
