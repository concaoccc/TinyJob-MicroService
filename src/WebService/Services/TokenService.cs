using WebService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebService.Services
{
    public class TokenService
    {
        private readonly AppSettings _appSettings;

        public TokenService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public string GetToken(UserCredential userCredential)
        {
            SecurityTokenDescriptor tokenDescriptor = GetTokenDescriptor(userCredential);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        private SecurityTokenDescriptor GetTokenDescriptor(UserCredential userCredential)
        {
            const int expiringDays = 7;

            byte[] securityKey = Encoding.UTF8.GetBytes(_appSettings.EncryptionKey);
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userCredential.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(expiringDays),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenDescriptor;
        }
    }
}
