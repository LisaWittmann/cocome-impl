using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>SaleTO</c> is a data transfer object
    /// of the class <see cref="Sale"/>
    /// </summary>
    public class SaleTO
    {
        [Required]
        public SaleElementTO[] SaleElements { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public float Payed { get; set; }

        public float Total { get; set; }

        public Store Store { get; set; }
    }
}
