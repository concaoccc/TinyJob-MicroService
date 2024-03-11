using WebService.Models;
using Common.Database.Repositories;
using Common.Database.PO;
using System.Security.Authentication;

namespace WebService.Services
{
    public class AuthenticationService
    {
        private readonly UserService userService;
        private readonly TokenService tokenService;

        public AuthenticationService(UserService userService, TokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        public string Authenticate(UserCredential userCredential)
        {
            userService.ValidateUserCredential(userCredential);
            string securityToken = tokenService.GetToken(userCredential);

            return securityToken;
        }
    }
}
