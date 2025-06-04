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

        public string Price { get; set; }

        public bool IsAllocated { get; set; }

        public string Driver { get; set; }
        public string Uid { get; set; }
        public string Status { get; set; }

        public string PendingDate{ get; set; }
        public string PendingTime { get; set; }

        public string OnWayDate { get; set; }
        public string OnWayTime { get; set; }

        public string AtCollectionDate { get; set; }
        public string AtCollectionTime { get; set; }

        public string AtCollectionNotes { get; set; }

        public string LoadedDate { get; set; }
        public string LoadedTime { get; set; }

        public string LoadedNotes { get; set; }

        public string AtDeliveryDate { get; set; }
        public string AtDeliveryTime { get; set; }

        public string AtDeliveryNotes { get; set; }

        public string DeliveredDate { get; set; }
        public string DeliveredTime { get; set; }

        public string DeliveredNotes { get; set; }

        public string ReceivedBy { get; set; }

        public string SigURL { get; set; }

        //[Required(ErrorMessage = "Collection Point is required")]
        public string Collection { get; set; }
        public string CollectionDate { get; set; }
        public string CollectionTime { get; set; }


        //[Required(ErrorMessage = "Delivery Point is required")]
        public string Delivery { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }   
    }
}
