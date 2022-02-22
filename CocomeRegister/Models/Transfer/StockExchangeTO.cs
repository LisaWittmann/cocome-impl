using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class StockExchangeTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public ExchangeElement[] Elements { get; set; }

        [Required]
        public Store Store { get; set; }

        [Required]
        public Store Provider { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        public bool Sended { get; set; }

        public bool Closed { get; set; }
    }
}
