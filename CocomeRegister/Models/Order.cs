using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    public class Order
    {

        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public Store Store { get; set; }

        [Required]
        public Provider Provider { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        [Required]
        public bool Delivered { get; set; }

        [Required]
        public bool Closed { get; set; }
    }
}
