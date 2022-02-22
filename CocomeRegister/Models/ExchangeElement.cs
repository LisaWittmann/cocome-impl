using System.ComponentModel.DataAnnotations;
namespace CocomeStore.Models
{
    public class ExchangeElement : TradeElement
    {
        [Required]
        public int StockExchangeId { get; set; }

        public StockExchange StockExchange { get; set; }
    }
}
