using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vehicle Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Vehicle Type is required")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Vehicle Make is required")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Vehicle Model is required")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Vehicle Year is required")]
        public string Year { get; set; }
        [Required(ErrorMessage = "Vehicle Registration is required")]
        public string RegNo { get; set; }
        [Required(ErrorMessage = "MOT Expiry Date is required")]
        public string MOTExpiry { get; set; }
        [Required(ErrorMessage = "Tax Expiry Date is required")]
        public string TaxExpiry { get; set; }
        [Required(ErrorMessage = "H&R Insurance Expiry Date is required")]
        public string HireAndRewardExpiry { get; set; }
        [Required(ErrorMessage = "GIT Insurance Expiry Date is required")]
        public string GITExpiry { get; set; }
        [Required(ErrorMessage = "GIT Limit is required")]
        public string GITLimit { get; set; }
        [Required(ErrorMessage = "Owner/Driver is required")]
        public string Driver { get; set; }
        public string Uid { get; set; }
        public bool IsValid { get; set; }

        public string ValidStatus { get; set; }

        public void UpdateVehicleStatus()
        {
            DateTime now = DateTime.Now;
            string motExpirySanitized = MOTExpiry?.Replace(".:", ":");
            string taxExpirySanitized = TaxExpiry?.Replace(".:", ":");
            string hireAndRewardExpirySanitized = HireAndRewardExpiry?.Replace(".:", ":");
            string gitExpirySanitized = GITExpiry?.Replace(".:", ":");

            if (!DateTime.TryParse(motExpirySanitized, out DateTime motExpiryDate) ||
                    !DateTime.TryParse(taxExpirySanitized, out DateTime taxExpiryDate) ||
                    !DateTime.TryParse(hireAndRewardExpirySanitized, out DateTime hireAndRewardExpiryDate) ||
                    !DateTime.TryParse(gitExpirySanitized, out DateTime gitExpiryDate))
            {
                // Handle invalid date format
                IsValid = false;
                ValidStatus = "Invalid Date Format";
                return;
            }

            if (motExpiryDate < now || taxExpiryDate < now || hireAndRewardExpiryDate < now || gitExpiryDate < now)
            {
                //  Vehicle Documents Out Of Date
                IsValid = false;
                ValidStatus = "Red Warning";
            }
            else if ((motExpiryDate > now && motExpiryDate <= now.AddDays(7)) ||
                    (taxExpiryDate > now && taxExpiryDate <= now.AddDays(7)) ||
                    (hireAndRewardExpiryDate > now && hireAndRewardExpiryDate <= now.AddDays(7)) ||
                    (gitExpiryDate > now && gitExpiryDate <= now.AddDays(7)))
            {
                //  Action Required in Next 7 Days
                IsValid = true;
                ValidStatus = "Amber Warning";
            }
            else if ((motExpiryDate > now.AddDays(7) && motExpiryDate <= now.AddDays(30)) ||
                     (taxExpiryDate > now.AddDays(7) && taxExpiryDate <= now.AddDays(30)) ||
                     (hireAndRewardExpiryDate > now.AddDays(7) && hireAndRewardExpiryDate <= now.AddDays(30)) ||
                     (gitExpiryDate > now.AddDays(7) && gitExpiryDate <= now.AddDays(30)))
            {
                //  Action Required in Next 30 Days
                IsValid = true;
                ValidStatus = "Yellow Warning";
            }
            else if (motExpiryDate > now.AddDays(30) &&
                     taxExpiryDate > now.AddDays(30) &&
                     hireAndRewardExpiryDate > now.AddDays(30) &&
                     gitExpiryDate > now.AddDays(30))
            {
                // No Action Required
                IsValid = true;
                ValidStatus = "Good";
            }
        }

        public void UpdateValidStatus()
        {
            ValidStatus = IsValid ? "Valid" : "Invalid";
        }
    }
}
