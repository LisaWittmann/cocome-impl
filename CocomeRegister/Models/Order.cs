using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Order</c>
    /// </summary>
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
    }
}
