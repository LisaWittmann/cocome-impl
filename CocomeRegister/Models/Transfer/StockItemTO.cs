using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class StockItemTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public Store Store { get; set; }

        [Required]
        public int Minimum { get; set; }
    }
}
