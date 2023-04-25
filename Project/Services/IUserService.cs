using Microsoft.AspNetCore.Mvc;

namespace Project.Services
{
    public interface IUserService
    {
        Task<IActionResult> AddUserToRole(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
    }
}
