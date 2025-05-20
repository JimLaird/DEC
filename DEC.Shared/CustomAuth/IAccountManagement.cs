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
        /// <summary>
        /// Creates a new user account with the provided credentials.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="password">The user's password</param>
        /// <param name="username">The user's display name</param>
        /// <returns>A FormResult indicating success or containing error details</returns>
        public Task<FormResult> RegisterAsync(string email, string password, string username);

        /// <summary>
        /// Authenticates a user with the provided credentials.
        /// </summary>
        /// <param name="email">The user's email address</param>
        /// <param name="password">The user's password</param>
        /// <returns>A FormResult indicating success or containing error messages</returns>
        public Task<FormResult> LoginAsync(string email, string password);

        /// <summary>
        /// Terminates the current user session and clears authentication state.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation</returns>
        public Task LogoutAsync();

        /// <summary>
        /// Verifies if the current user has a valid authenticated session.
        /// </summary>
        /// <returns>True if the user is authenticated, otherwise false</returns>
        public Task<bool> CheckAuthenticatedAsync();
    }
}
