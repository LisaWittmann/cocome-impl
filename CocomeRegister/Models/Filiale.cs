using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Filiale
    {
        [Required]
        public int Id { get; }

        [Required]
        public string Name { get; set; }

        public string Stadt { get; set; }

        public long Postleitzahl { get; set; }
    }
}
