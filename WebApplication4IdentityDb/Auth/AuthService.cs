using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using WebApplication4IdentityDb.Models;

namespace WebApplication4IdentityDb.Auth
{
    public class AuthService : IAuthService
    {
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            // _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContextAccessor;
        }

        public async Task Logout()
        {
            await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // await _httpContext.HttpContext.SignOutAsync(.AuthenticationScheme);

        }

        async Task<bool> IAuthService.AddUserClaim(string user, Claim claim)
        {
            var identityUser = await _userManager.FindByNameAsync(user);
            if (identityUser is null)
            {
                return false;
            }

            var result = await _userManager.AddClaimAsync(identityUser, claim);
            return result.Succeeded;
        }

        Task IAuthService.GenerateCookieAuthentication(string userName)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IAuthService.Login(LoginUser credentials)
        {
            var user = await _userManager.FindByNameAsync(credentials.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, credentials.Password, false);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }



        //private async Task<List<Claim>> GetClaims(string username)
        //{
        //    var user = await _userManager.FindByNameAsync(username);

        //    var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.Name, username),
        //    };

        //    claims.AddRange(GetClaimsSeperated(await _userManager.GetClaimsAsync(user)));
        //    var roles = await _userManager.GetRolesAsync(user);

        //    foreach (var role in roles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));

        //        var identityRole = await _roleManager.FindByNameAsync(role);
        //        claims.AddRange(GetClaimsSeperated(await _roleManager.GetClaimsAsync(identityRole)));
        //    }

        //    return claims;
        //}
        //private List<Claim> GetClaimsSeperated(IList<Claim> claims)
        //{
        //    var result = new List<Claim>();
        //    foreach (var claim in claims)
        //    {
        //        result.AddRange(claim.DeserializePermissions().Select(t => new Claim(claim.Type, t.ToString())));
        //    }
        //    return result;
        //}

        //public async Task<string> GenerateTokenString(string user, JwtConfiguration jwtConfig)
        //{
        //    var claims = await GetClaims(user);

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));

        //    var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        //    var securityToken = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(60),
        //        issuer: jwtConfig.Issuer,
        //        audience: jwtConfig.Audience,
        //        signingCredentials: signingCred);

        //    string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        //    return tokenString;
        //}
        async Task<bool> IAuthService.RegisterUser(LoginUser user)
        {
            var identityUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            return result.Succeeded;
        }

        Task<bool> IAuthService.RemoveUserClaim(string user, Claim claim)
        {
            throw new NotImplementedException();
        }
    }
}
