using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Order</c>
    /// </summary>
    public class Order : Trade<Provider>
    {
    }

    /// <summary>
    /// class <c>OrderElement</c>
    /// </summary>
    public class OrderElement : TradeElement
    {
        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
