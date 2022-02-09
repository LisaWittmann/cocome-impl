using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class CashDesk
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Store Store { get; set; }
    }
}
