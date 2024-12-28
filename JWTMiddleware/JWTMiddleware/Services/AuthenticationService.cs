using JWTMiddleware.Helper;
using JWTMiddleware.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace JWTMiddleware.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private List<User> _users = new List<User> {
            new User {
                Id = 1, 
                FirstName = "mytest", 
                LastName = "User", 
                UserName = "mytestuser",
                Roles= new List<Role>{Role.Customer} , 
                Password = "test123"
            }
        };
        public AuthenticationService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;            
        }
        public AuthenticateResponse authenticate(AuthenticateRequest model)
        {
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();

            var user = _users.FirstOrDefault(x => x.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(model.Password));
            if (user == null) {
                return null;
            }
            var generatedToken = generateJwtToken(user);
            authenticateResponse.Token = generatedToken;
            return authenticateResponse;

        }

        private string generateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_appSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>() {
                new Claim("Id",Convert.ToString(user.Id)),
                new Claim(JwtRegisteredClaimNames.Sub,"Test"),
                new Claim(JwtRegisteredClaimNames.Email,"test@gmail.com"),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim("Role", Convert.ToString(role)));
            }
            var token = new JwtSecurityToken(_appSettings.Issuer, _appSettings.Issuer, claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

