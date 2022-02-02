using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Rabatt
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Produkt Produkt { get; set; }

        [Required]
        public float Hohe { get; set; }

        //public Filiale Filiale { get; set; }
    }
}
