using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    public class StockItem
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public Store Store { get; set; }

        public int Minimum { get; set; }
    }
}
