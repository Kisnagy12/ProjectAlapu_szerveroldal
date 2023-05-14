using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.DataTransferObjects;
using Project.Entities;
using Project.Services;
using Project.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthenticationController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Register user by the provided registration data and return the status based on, whether the process was successful or not
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        [AllowAnonymous]
        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> RegisterUser(UserRegistrationDTO userForRegistration)
        {
            if (_userManager.Users.Any(u => u.UserName == userForRegistration.Username))
            {
                return BadRequest("Username already exists!");
            }
            var user = new ApplicationUser
            {
                UserName = userForRegistration.Username,
                Email = userForRegistration.Email,
                NormalizedUserName=userForRegistration.Username.Normalize(),
            };
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            await _userService.AddUserToRole(user.Id, Roles.USER);

            return new JsonResult(user);
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> AddUserToAdminRole(string userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user != null)
            {
                await _userService.AddUserToRole(user.Id, Roles.ADMIN);
                return Ok(new JsonResult(new UserViewModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,
                }));
            }

            return BadRequest("User not found");

        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> RemoveUserFromAdmins(string userId)
        {
            var user = await _userService.GetUserById(userId);

            if (user != null)
            {
                await _userService.RemoveAdminRole(user.Id, Roles.ADMIN);
                return Ok(new JsonResult(new UserViewModel
                {
                    Email = user.Email,
                    UserName = user.UserName,
                    UserId = user.Id,
                }));
            }

            return BadRequest("User not found");

        }

        [Authorize]
        [HttpGet]
        [EnableCors]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserViewModel> userList = new();
            int num = 0;
            int size = 100;

            while (true)
            {
                var users = await _userManager.Users.Skip(num * size).Take(size).ToListAsync();
                if (users.Count == 0)
                {
                    break;
                }
                foreach (var user in users)
                {
                    var userRoles = await _userService.GetUserRolesAsync(user.Id);
                    if (userRoles != null)
                    {
                        userList.Add(new UserViewModel
                        {
                            UserId=user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            UserRoles = userRoles,
                        });
                    }
                    else
                    {
                        userList.Add(new UserViewModel
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                        });
                    }
                }
                num++;
            }
            return new JsonResult(userList);
        }

        /// <summary>
        /// Login the user by username and password and return a generated JWT token
        /// </summary>
        /// <param name="userLoginDTO"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _userManager.FindByNameAsync(userLoginDTO.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                var userRoles = await _userService.GetUserRolesAsync(user.Id);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> GetPrincipalByToken(string? token)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var userRoles=new List<string>();
            var user = new ApplicationUser();
            if (principal != null)
            {
                user = await _userManager.FindByNameAsync(principal.Identity!.Name);
                userRoles = await _userService.GetUserRolesAsync(user.Id);
            }
            else
            {
                return BadRequest();
            }
            return new JsonResult(new{ user, userRoles});
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Logout(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            /*var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;*/

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(4),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
