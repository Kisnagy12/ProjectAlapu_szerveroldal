using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DbContexts;
using Project.Entities;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly SurvivalAnalysisContext _survivalAnalysisContext;

        public UserService(SurvivalAnalysisContext survivalAnalysisContext)
        {
            _survivalAnalysisContext = survivalAnalysisContext;
        }
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            var role = _survivalAnalysisContext.IdentityRole.Where(x => x.NormalizedName == roleName).FirstOrDefault();
            var newUserRole = new IdentityUserRole
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                RoleId = role.Id
            };
            _survivalAnalysisContext.Add(newUserRole);
            await _survivalAnalysisContext.SaveChangesAsync();

            return new JsonResult(newUserRole);
        }

        public async Task<IActionResult> RemoveAdminRole(string userId, string roleName)
        {
            var role = _survivalAnalysisContext.IdentityRole.Where(x => x.NormalizedName == roleName).FirstOrDefault();
            var userRole=_survivalAnalysisContext.IdentityUserRole.Where(x => x.UserId == userId && x.RoleId==role.Id).FirstOrDefault();
            _survivalAnalysisContext.Entry(userRole).State = EntityState.Deleted;
            await _survivalAnalysisContext.SaveChangesAsync();

            return new JsonResult(userRole);
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var userRoles = await _survivalAnalysisContext.IdentityUserRole.Where(x => x.UserId == userId).ToListAsync();

            var roles = new List<string>();

            foreach (var userRole in userRoles)
            {
                roles.Add(_survivalAnalysisContext.IdentityRole.Where(x => x.Id == userRole.RoleId).FirstOrDefault().NormalizedName);
            }

            return roles;
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user= await _survivalAnalysisContext.ApplicationUser.Where(x => x.Id == userId).FirstOrDefaultAsync();

            return user;
        }
    }
}
