namespace DEC.Shared.Services
{
    /// <summary>
    /// Defines methods to retrieve information about the form factor and platform of a device or system.
    /// </summary>
    public interface IFormFactor
    {
        public string GetFormFactor();
        public string GetPlatform();
    }
}
