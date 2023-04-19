using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DbContexts;
using Project.Entities;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly CourseStatisticsContext _courseStatisticsContext;

        public UserService(CourseStatisticsContext courseStatisticsContext)
        {
            _courseStatisticsContext = courseStatisticsContext;
        }
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var role=_courseStatisticsContext.IdentityRole.Where(x=>x.NormalizedName == roleName).FirstOrDefault();
            var newUserRole = new IdentityUserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RoleId = role.Id
            };
            _courseStatisticsContext.Add(newUserRole);
            await _courseStatisticsContext.SaveChangesAsync();

            return new JsonResult(newUserRole);
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var userRoles = await _courseStatisticsContext.IdentityUserRole.Where(x => x.UserId == userId).ToListAsync();

            var roles = new List<string>();

            foreach (var userRole in userRoles)
            {
                roles.Add(_courseStatisticsContext.IdentityRole.Where(x => x.Id == userRole.RoleId).FirstOrDefault().NormalizedName);
            }

            return roles;
        }
    }
}
