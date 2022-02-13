using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    public class SaleElement
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
        public int SaleId { get; set; }

        public Sale Sale { get; set; }

        public Discount Discount { get; set; }
    }

    public class Sale
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Store Store { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

    }
}
