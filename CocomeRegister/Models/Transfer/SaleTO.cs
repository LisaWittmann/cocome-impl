using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class SaleTO
    {
        [Required]
        public SaleElementTO[] SaleElements { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public float HandedCash { get; set; }
    }
}
