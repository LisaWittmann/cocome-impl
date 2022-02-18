using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class SaleElementTO
    {
        [Required]
        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public int HandedCash { get; set; }
    }

    public enum PaymentMethod
    {
        Cash,
        Card
    }
}
