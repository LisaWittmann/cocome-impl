using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>ProductTO</c> is a data transfer object of class <see cref="Product"/>
    /// </summary>
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
