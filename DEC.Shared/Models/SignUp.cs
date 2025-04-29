using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    [RequiresUnreferencedCode("Necessary because of RangeAttribute usage")]
    public class SignUp
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(8, ErrorMessage = "Username is too short (8 characters minimum")]
        public string UserName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Email Address is too short")]
        [StringLength(40, ErrorMessage = "Email Address too long (40 character limit)")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password is too short (8 characters minimum")]
        public string Password { get; set; }
    }
}
