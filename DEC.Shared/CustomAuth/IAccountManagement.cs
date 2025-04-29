using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEC.Shared.Models;

namespace DEC.Shared.CustomAuth
{
    public interface IAccountManagement
    {
        public Task<FormResult> RegisterAsync(string email, string password, string username);
        public Task<FormResult> LoginAsync(string email, string password);
        public Task LogoutAsync();
        public Task<bool> CheckAuthenticatedAsync();
    }
}
