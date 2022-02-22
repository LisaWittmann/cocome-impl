using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>SaleElement</c>
    /// </summary>
    public class SaleElement : TradeElement
    {
        [Required]
        public int SaleId { get; set; }

        public Sale Sale { get; set; }

        public float Discount { get; set; }
    }
}
