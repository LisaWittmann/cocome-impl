using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class VerkaufsEreignis
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Filiale Filiale { get; set; }

        [Required]
        public VerkaufsElement[] VerkaufsElements { get; set; }

        [Required]
        public DateTime Zeitpunkt { get; set; }

    }
}
