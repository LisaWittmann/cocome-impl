using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// abstract class <c>TradeTO</c> is a data transfer object of class <see cref="Trade{T}"/>
    /// </summary>
    /// <typeparam name="T">class of the trades provider</typeparam>
    public abstract class TradeTO<T>
    {
        public int Id { get; set; }

        [Required]
        public Store Store { get; set; }

        [Required]
        public T Provider { get; set; }

        [Required]
        public TradeElementTO[] Elements { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        public bool Closed { get; set; }
    }

    /// <summary>
    /// class <c>OrderTO</c> is a data transfer object of class <see cref="Order"/>
    /// and extends class <see cref="TradeTO{Provider}"/> for provider class
    /// <see cref="Provider"/>
    /// </summary>
    public class OrderTO : TradeTO<Provider> { }

    /// <summary>
    /// class <c>StockExchangeTO</c> is a data transfer object of class
    /// <see cref="StockExchange"/> and extends class <see cref="TradeTO{Store}"/>
    /// for provider class <see cref="Store"/>
    /// </summary>
    public class StockExchangeTO : TradeTO<Store>
    {
        public bool Sended { get; set; }
    }
}
