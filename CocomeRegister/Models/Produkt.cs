using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Produkt
    {
        [Required]
        public int Id { get; }

        [Required]
        public string Name { get; set; }

        public string Beschreibung { get; set; }

        [Required]
        public float Preis { get; set; }

        //public ProduktGruppe gruppe { get; set; }
    }
}
