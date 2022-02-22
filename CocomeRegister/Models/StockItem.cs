using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>StockItem</c>
    /// </summary>
    public class StockItem
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StoreId { get; set; }

        public Store Store { get; set; }

        public int Minimum { get; set; }
    }
}
