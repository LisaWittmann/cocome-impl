using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CocomeStore.Models
{
    /// <summary>
    /// class <c>OrderElement</c>
    /// </summary>
    public class OrderElement
    {
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
