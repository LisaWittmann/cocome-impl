using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Discount
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public float Percentage { get; set; }

        //public Filiale Filiale { get; set; }
    }
}
