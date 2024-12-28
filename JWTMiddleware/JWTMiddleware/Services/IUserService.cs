using JWTMiddleware.Model;

namespace JWTMiddleware.Services
{
    public interface IUserService
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
    }
}
