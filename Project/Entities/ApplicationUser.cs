using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Project.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
