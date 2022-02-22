using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>SaleElementTO</c> is a data transfer object
    /// of the class <see cref="SaleElement"/>
    /// </summary>
    public class SaleElementTO
    {
        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }

        public float Discount { get; set; }
    }
}
