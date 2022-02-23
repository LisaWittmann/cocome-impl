using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>Report</c> is a data transfer object to unite a dataset
    /// with a describing label
    /// </summary>
    public class Report
    {
        [Required]
        public string Label { get; set; }

        [Required]
        public double[] Dataset { get; set; }
    }
}