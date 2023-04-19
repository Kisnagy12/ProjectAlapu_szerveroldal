using Microsoft.AspNetCore.Identity;

namespace Project.Entities
{
    public class IdentityUserRole
    {
        public string Id { get; set; }
        public string? UserId { get; set; } 
        public string? RoleId { get; set; } 
    }
}
