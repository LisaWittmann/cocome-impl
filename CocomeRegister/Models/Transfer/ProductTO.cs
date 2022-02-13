using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class ProductTO
    {
        [Required]
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
        public Provider Provider { get; set; }
    }
}
