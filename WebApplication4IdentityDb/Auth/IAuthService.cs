using System.Security.Claims;

namespace WebApplication4IdentityDb.Auth
{
    public interface IAuthService
    {
        Task<bool> Login(LoginUser credentials);

        Task<bool> RegisterUser(LoginUser credentials);
        Task<bool> AddUserClaim(string user, Claim claim);
        Task Logout();

        Task<bool> RemoveUserClaim(string user, Claim claim);
        Task GenerateCookieAuthentication(string userName);
    }
}
