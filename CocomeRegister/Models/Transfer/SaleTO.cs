using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>SaleTO</c> is a data transfer object of class <see cref="Sale"/>
    /// </summary>
    public class SaleTO
    {
        [Required]
        public SaleElementTO[] SaleElements { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public float Payed { get; set; }

        [Required]
        public float Total { get; set; }

        public Store Store { get; set; }
    }
}
