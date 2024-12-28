using JWTMiddleware.Model;

namespace JWTMiddleware.Services
{
    public interface IAuthenticationService
    {
        AuthenticateResponse authenticate(AuthenticateRequest model);
    }
}
