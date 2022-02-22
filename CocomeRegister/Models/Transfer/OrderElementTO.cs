using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>OrderElementTO</c> is a data transfer object
    /// of the class <see cref="OrderElement"/>
    /// </summary>
    public class OrderElementTO
    {
        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
