using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class VerkaufsElement
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Produkt Produkt { get; set; }

        [Required]
        public int Anzahl { get; set; }

        public Rabatt Rabatt { get; set; }
    }
}
