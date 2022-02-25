using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CocomeStore.Models.Transfer
{
    public class UserTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public Store Store { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
