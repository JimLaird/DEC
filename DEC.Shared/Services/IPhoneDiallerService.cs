using System.Threading.Tasks;

namespace DEC.Shared.Services
{
    public interface IPhoneDiallerService
    {
        Task<bool> PlacePhoneCallAsync(string phoneNumber, string displayName = null);
    }
}
