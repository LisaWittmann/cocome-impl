using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>Store</c> defines the information of an enterprises store instance
    /// </summary>
    public class Store
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string City { get; set; }

        public long PostalCode { get; set; }
    }
}
