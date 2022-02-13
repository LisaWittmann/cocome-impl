using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class OrderElementTO
    {
        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }
    }
}
