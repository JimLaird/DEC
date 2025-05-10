using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;
using DEC.Shared.Services;
using Microsoft.Maui.Devices;

namespace DEC.Services
{
    public class PhoneDiallerService : IPhoneDiallerService
    {
        public async Task<bool> PlacePhoneCallAsync(string phoneNumber, string displayName = null)
        {
            try
            {
                // Format phone number - ensure it's properly formatted for dialing  
                phoneNumber = phoneNumber.Replace("+", "").Replace(" ", "").Replace("-", "");

                // Check if dialing is supported on the device  
                if (PhoneDialer.IsSupported)
                {
                    // Open the phone dialer  
                    PhoneDialer.Open(phoneNumber);
                    return true;
                }
                else
                {
                    // Fall back for web/Windows where direct dialing may not be supported  
                    // Use browser-based tel: protocol  
                    await Microsoft.Maui.ApplicationModel.Launcher.OpenAsync($"tel:{phoneNumber}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Phone dialing error: {ex.Message}");
                return false;
            }
        }
    }
}
