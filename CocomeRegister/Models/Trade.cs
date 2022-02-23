using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Trade</c> defines the object structure for a trade
    /// of a store
    /// </summary>
    /// <typeparam name="T">type of provider</typeparam>
    public abstract class Trade<T>
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Store Store { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public T Provider { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }
    }

    /// <summary>
    /// class <c>Order</c> extends class <see cref="Trade{Provider}"/> with the
    /// specific type <see cref="Provider"/> and defines a trade between a store
    /// and a provider
    /// </summary>
    public class Order : Trade<Provider> { }

    /// <summary>
    /// class <c>StockExchange</c> extends class <see cref="Trade{Store}"/> with
    /// the specific type <see cref="Store"/> and defines a trade between two stores
    /// </summary>
    public class StockExchange : Trade<Store> { }
}
