using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class StockExchangeTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public ExchangeElement[] ExchangeElements { get; set; }

        [Required]
        public Store ReceivingStore { get; set; }

        [Required]
        public Store SendingStore { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        public bool Closed { get; set; }
    }
}
