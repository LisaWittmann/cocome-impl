using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Product</c>
    /// </summary>
    public class Product
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public float Price { get; set; }

        [Required]
        public float SalePrice { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }
    }
}
