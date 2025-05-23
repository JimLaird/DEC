using System.Threading.Tasks;

namespace DEC.Shared.Services
{
    /// <summary>
    /// Defines a service for placing phone calls.
    /// </summary>
    /// <remarks>This interface provides a method to initiate a phone call to a specified phone number. 
    /// Implementations of this interface may vary depending on the platform or environment.</remarks>
    public interface IPhoneDiallerService
    {
        /// <summary>
        /// Initiates an asynchronous phone call to the specified phone number.
        /// </summary>
        /// <remarks>The success of the call initiation depends on the availability of the underlying
        /// telephony service.  Ensure that the calling system is properly configured before invoking this
        /// method.</remarks>
        /// <param name="phoneNumber">The phone number to call. This must be a valid phone number in international format.</param>
        /// <param name="displayName">An optional display name to associate with the call. If provided, this name will be shown to the recipient, 
        /// if supported by the calling system.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the call was
        /// successfully initiated; otherwise, <see langword="false"/>.</returns>
        Task<bool> PlacePhoneCallAsync(string phoneNumber, string displayName = null);
    }
}
