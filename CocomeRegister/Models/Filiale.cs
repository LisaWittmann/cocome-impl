using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Filiale
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Stadt { get; set; }
    }
}
