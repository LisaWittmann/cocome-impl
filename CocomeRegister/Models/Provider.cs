using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Provider</c> defines the information of an enterprises product
    /// provider
    /// </summary>
    public class Provider
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
