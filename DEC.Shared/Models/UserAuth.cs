using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    public class UserAuth
    {
        public Info Info { get; set; }
        public Credential Credential { get; set; }

    }

    public class Info
    {
        public string Uid { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool IsEmailVerified { get; set; }
        public object PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class Credential
    {
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Created { get; set; }
        public int ExpiresIn { get; set; }
        public int ProviderType { get; set; }
    }

    // Added these 2 classes
    public class UserInfo
    {
        public User[] users { get; set; }
    }

    public class User
    {
        public string LocalId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string CustomAttributes { get; set; }
    }
    // Added these 2 classes

}
