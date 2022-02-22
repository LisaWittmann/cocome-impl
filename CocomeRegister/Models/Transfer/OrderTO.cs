using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    /// <summary>
    /// class <c>OrderTO</c> is a data transfer object
    /// of the class <see cref="Order"/>
    /// </summary>
    public class OrderTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public OrderElementTO[] OrderElements { get; set; }

        [Required]
        public Store Store { get; set; }

        [Required]
        public Provider Provider { get; set; }

        [Required]
        public DateTime PlacingDate { get; set; }

        public DateTime DeliveringDate { get; set; }

        public bool Closed { get; set; }
    }
}
