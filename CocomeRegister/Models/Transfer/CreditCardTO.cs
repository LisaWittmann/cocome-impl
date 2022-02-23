using System;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class CreditCardTO
    {
        [Required]
        public string Number;

        [Required]
        public long Pin;
    }
}
