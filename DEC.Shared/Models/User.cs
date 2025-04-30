using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
