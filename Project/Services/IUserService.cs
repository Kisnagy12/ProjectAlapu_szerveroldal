using Microsoft.AspNetCore.Mvc;
using Project.Entities;

namespace Project.Services
{
    public interface IUserService
    {
        Task<IActionResult> AddUserToRole(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<ApplicationUser> GetUserById(string userId);
    }
}
