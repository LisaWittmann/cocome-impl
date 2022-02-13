using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    public class OrderElement
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }
    }

    public class Order
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Store Store { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        public bool Delivered { get; set; }

        public bool Closed { get; set; }
    }
}
