using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Lieferant
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
