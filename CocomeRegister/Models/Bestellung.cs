using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Bestellung
    {

        [Required]
        public int Id { get; set; } 

        [Required]
        public Produkt Produkt { get; set; }

        [Required]
        public int Anzahl { get; set; }

        [Required]
        public Filiale Filiale { get; set; }

        [Required]
        public Lieferant Lieferant { get; set; }

        [Required]
        public DateTime BestellZeitpunkt { get; set; }

        public DateTime LieferZeitpunkt { get; set; }

        [Required]
        public bool Geliefert { get; set; }

        [Required]
        public bool Geschlossen { get; set; }
    }
}
