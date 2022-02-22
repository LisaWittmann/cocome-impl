using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// abstract class <c>TradeElementTO</c> is a data transfer object for class
    /// <see cref="TradeElement"/>
    /// </summary>
    public abstract class TradeElementTO
    {
        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }
    }

    /// <summary>
    /// class <c>OrderElementTO</c> is a data transfer object for class
    /// <see cref="OrderElement"/> and extends class <see cref="TradeElementTO"/>
    /// </summary>
    public class OrderElementTO : TradeElementTO { }

    /// <summary>
    /// class <c>ExchangeElementTO</c> is a data transfer object for class
    /// <see cref="ExchangeElement"/> and extends class <see cref="TradeElementTO"/>
    /// </summary>
    public class ExchangeElementTO : TradeElementTO { }

    /// <summary>
    /// class <c>SaleElementTO</c> is a data transfer object for class
    /// <see cref="SaleElement"/> and extends class <see cref="TradeElementTO"/>
    /// </summary>
    public class SaleElementTO : TradeElementTO
    {
        public float Discount { get; set; }
    }
}
