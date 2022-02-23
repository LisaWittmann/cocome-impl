using System;
using System.Collections.Generic;
using System.Linq;
using CocomeStore.Models;
using CocomeStore.Models.Transfer;

namespace CocomeStore.Services.Mapping
{
    /// <summary>
    /// class <c>ModelMapper</c> is an implementation of <see cref="IModelMapper"/>
    /// and provides methods to convert objects into transfer objects or to convert
    /// an objects transfer object into the origin class
    /// </summary>
    public class ModelMapper : IModelMapper
    {
        /// <summary>
        /// method <c>CreateOrderElement</c> creates a new order element out of
        /// a given order and the order element transfer object
        /// </summary>
        /// <param name="order">related order object</param>
        /// <param name="orderElementTO">
        /// transfer object containing the order elements information
        /// </param>
        /// <returns>new order element instance</returns>
        public OrderElement CreateOrderElement(Order order, OrderElementTO orderElementTO)
        {
            return new()
            {
                ProductId = orderElementTO.Product.Id,
                Amount = orderElementTO.Amount,
                Order = order,
            };
        }

        /// <summary>
        /// method <c>CreateOrderElementTO</c> creates a new order element
        /// transfer object out of the given order element
        /// </summary>
        /// <param name="orderElement">
        /// order element instance to convert in transfer object
        /// </param>
        /// <returns>new order element transfer object instance</returns>
        public OrderElementTO CreateOrderElementTO(OrderElement orderElement)
        {
            return new()
            {
                Product = orderElement.Product,
                Amount = orderElement.Amount,
            };
        }

        /// <summary>
        /// method <c>CreateOrder</c> creates a new order out of the given order
        /// transfer object
        /// </summary>
        /// <param name="orderTO">
        /// transfer object containing the orders information
        /// </param>
        /// <returns>new order instance</returns>
        public Order CreateOrder(OrderTO orderTO)
        {
            return new()
            {
                StoreId = orderTO.Store.Id,
                ProviderId = orderTO.Provider.Id,
                PlacingDate = orderTO.PlacingDate,
            };
        }

        /// <summary>
        /// method <c>CreateOrderTO</c> creates a new order transfer object
        /// instance out of the given order and its belonging order elements
        /// </summary>
        /// <param name="order">
        /// order object to convert in transfer object
        /// </param>
        /// <param name="orderElements">
        /// related order elements of the order object to convert
        /// </param>
        /// <returns>new order transfer object instance</returns>
        public OrderTO CreateOrderTO(Order order, IEnumerable<OrderElement> orderElements)
        {
            var elements = orderElements
                 .DefaultIfEmpty()
                 .Select(element => CreateOrderElementTO(element))
                 .ToArray();

            return new()
            {
                Id = order.Id,
                Elements = elements.ToArray(),
                Store = order.Store,
                Provider = order.Provider,
                PlacingDate = order.PlacingDate,
                DeliveringDate = order.DeliveringDate,
                Closed = (order.DeliveringDate != DateTime.MinValue)
            };
        }

        /// <summary>
        /// method <c>CreateProduct</c> creates a new product out of a given
        /// product transfer object instance
        /// </summary>
        /// <param name="productTO">
        /// transfer object containing the products information
        /// </param>
        /// <returns>new product instance</returns>
        public Product CreateProduct(ProductTO productTO)
        {
            return new()
            {
                Name = productTO.Name,
                Price = productTO.Price,
                SalePrice = productTO.SalePrice,
                ProviderId = productTO.Provider.Id,
                Description = productTO.Description,
                ImageUrl = productTO.ImageUrl

            };
        }

        /// <summary>
        /// method <c>CreateProductTO</c> creates a new product transfer object
        /// based on the given products data
        /// </summary>
        /// <param name="product">
        /// product instance to convert into transfer object
        /// </param>
        /// <returns>new product transfer object instance</returns>
        public ProductTO CreateProductTO(Product product)
        {
            return new()
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

        /// <summary>
        /// method <c>CreateSale</c> creates a new sale based on hte given data
        /// of a sale transfer object
        /// </summary>
        /// <param name="saleTO">transfer object containing the data</param>
        /// <returns>new sale instance</returns>
        public Sale CreateSale(SaleTO saleTO)
        {
            return new()
            {
                StoreId = saleTO.Store.Id,
                TimeStamp = saleTO.TimeStamp,
                PaymentMethod = saleTO.PaymentMethod
            };
        }

        /// <summary>
        /// method <c>CreateSaleElement</c> creates a new sale element based on
        /// the given data of a sale transfer object, the related sale and the
        /// related storeId
        /// </summary>
        /// <param name="sale">related sale</param>
        /// <param name="storeId">related store by id</param>
        /// <param name="saleElementTO">transfer object containing the data</param>
        /// <returns>new sale element instance</returns>
        public SaleElement CreateSaleElement(Sale sale, int storeId, SaleElementTO saleElementTO)
        {
            return new()
            {
                ProductId = saleElementTO.Product.Id,
                Amount = saleElementTO.Amount,
                Sale = sale,
                Discount = saleElementTO.Discount
            };
        }

        /// <summary>
        /// method <c>CreateExchangeElementTO</c> creates a new exchange element
        /// transfer object based on the given ecxchange element data
        /// </summary>
        /// <param name="exchangeElement">
        /// exchange instance to convert into transfer object
        /// </param>
        /// <returns>new exchange element transfer object instance</returns>
        public ExchangeElementTO CreateExchangeElementTO(ExchangeElement exchangeElement)
        {
            return new()
            {
                Product = exchangeElement.Product,
                Amount = exchangeElement.Amount
            };
        }

        /// <summary>
        /// method <c>CreateStockExchangeTO</c> creates a new stock exchange
        /// transfer object based on the given exchange data
        /// </summary>
        /// <param name="stockExchange">
        /// exchange instance to convert into transfer object
        /// </param>
        /// <param name="exchangeElements">
        /// related exchange elements to stock exchange object
        /// </param>
        /// <returns>new stock exchange transfer object instance</returns>
        public StockExchangeTO CreateStockExchangeTO(
           StockExchange stockExchange,
           IEnumerable<ExchangeElement> exchangeElements
        )
        {
            var elements = exchangeElements
                 .DefaultIfEmpty()
                 .Select(element => CreateExchangeElementTO(element))
                 .ToArray();

            return new()
            {
                Id = stockExchange.Id,
                Store = stockExchange.Store,
                Provider = stockExchange.Provider,
                Elements = elements.ToArray(),
                PlacingDate = stockExchange.PlacingDate,
                DeliveringDate = stockExchange.DeliveringDate,
                Closed = stockExchange.DeliveringDate != DateTime.MinValue,
                Sended = stockExchange.PlacingDate != DateTime.MinValue
            };
        }

        /// <summary>
        /// method <c>UpdateProduct</c> syncronizes the given products data with
        /// the data of the transfer object
        /// </summary>
        /// <param name="product">product to modify</param>
        /// <param name="productTO">
        /// transfer object containing the updated data
        /// </param>
        public void UpdateProduct(Product product, ProductTO productTO)
        {
            product.Name = productTO.Name;
            product.Price = productTO.Price;
            product.SalePrice = productTO.SalePrice;
            product.Description = productTO.Description;
            product.ImageUrl = productTO.ImageUrl;
            product.ProviderId = productTO.Provider.Id;
        }
    }
}
