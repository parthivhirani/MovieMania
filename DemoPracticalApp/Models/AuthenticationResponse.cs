using Microsoft.AspNetCore.Identity;

namespace DemoPracticalApp.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public string Expiration { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}
