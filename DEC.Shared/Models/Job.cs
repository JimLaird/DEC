using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    public class Job
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Job Summary is required")]
        public string Summary { get; set; }

        public bool IsAllocated { get; set; }

        public string Driver { get; set; }
        public string Uid { get; set; }
        public string Status { get; set; }

        //[Required(ErrorMessage = "Collection Point is required")]
        public string Collection { get; set; }
        
        public string CollectionTime { get; set; }
        
        //[Required(ErrorMessage = "Delivery Point is required")]
        public string Delivery { get; set; }
        
        public string DeliveryTime { get; set; }   
    }
}
