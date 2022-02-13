using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    public class Statistic
    {
        [Required]
        public int Key { get; set; }

        [Required]
        public double[] Dataset { get; set; }
    }
}
