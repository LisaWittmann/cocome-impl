using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>Statistic</c> is a data transfer object to unite a dataset
    /// with a describing label
    /// </summary>
    public class Statistic
    {
        [Required]
        public string Label { get; set; }

        [Required]
        public double[] Dataset { get; set; }
    }
}