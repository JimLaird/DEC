
using System.ComponentModel.DataAnnotations;

namespace DEC.Shared.Models
{
    public class NonFactored
    {
        
        public string Id { get; set; }

        [Required(ErrorMessage = "Client Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CX Number is required")]
        public string Cx { get; set; }
    }
}
