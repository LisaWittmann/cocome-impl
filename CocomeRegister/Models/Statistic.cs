using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Statistic</c>
    /// </summary>
    public class Statistic
    {
        [Required]
        public string Label { get; set; }

        [Required]
        public double[] Dataset { get; set; }
    }
}