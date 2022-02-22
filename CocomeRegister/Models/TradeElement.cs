using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CocomeStore.Models
{
    /// <summary>
    /// abstract class <c>TradeElement</c> defines the stucture of an object
    /// for a <see cref="Trade{T}"/>
    /// </summary>
    public abstract class TradeElement
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }
    }

    /// <summary>
    /// class <c>OrderElement</c> extends the class <see cref="TradeElement"/>
    /// for the specific trade type <see cref="Models.Order"/>
    /// </summary>
    public class OrderElement : TradeElement
    {
        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }
    }

    /// <summary>
    /// class <c>ExchangeElement</c> extends the class <see cref="TradeElement"/>
    /// for the specific trade type <see cref="Models.StockExchange"/>
    /// </summary>
    public class ExchangeElement : TradeElement
    {
        [Required]
        public int StockExchangeId { get; set; }

        public StockExchange StockExchange { get; set; }
    }

    /// <summary>
    /// class <c>SaleElement</c> extends the class <see cref="TradeElement"/>
    /// for a <see cref="Models.Sale"/>
    /// </summary>
    public class SaleElement : TradeElement
    {
        [Required]
        public int SaleId { get; set; }

        public Sale Sale { get; set; }

        public float Discount { get; set; }
    }
}
