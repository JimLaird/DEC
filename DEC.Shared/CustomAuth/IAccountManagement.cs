using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEC.Shared.Models;

namespace DEC.Shared.CustomAuth
{
    /// <summary>
    /// Defines authentication operations for user account management in Blazor applications.
    /// Implementations handle user registration, authentication, and session management.
    /// </summary>
    public interface IAccountManagement
    {
        Task<bool> CheckAuthenticatedAsync();
        Task<FormResult> LoginAsync(string email, string password);
        Task<FormResult> RegisterAsync(string email, string password, string username);
        Task LogoutAsync();
        Task<UserAuth> GetUserAuthAsync();
        Task<bool> IsTokenValidAsync();
        Task<bool> RefreshTokenAsync();

        /// <summary>
        /// Gets a valid Firebase authentication token, refreshing if necessary.
        /// </summary>
        /// <returns>A valid Firebase ID token or empty string if authentication fails</returns>
        Task<string> GetAuthTokenAsync();
    }
}
