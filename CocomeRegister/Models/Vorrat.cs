using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Vorrat
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Produkt Produkt { get; }

        [Required]
        public int Anzahl { get; set; }

        [Required]
        public Filiale Filiale { get; set; }

        public int MindestVorrat { get; set; }
    }
}
