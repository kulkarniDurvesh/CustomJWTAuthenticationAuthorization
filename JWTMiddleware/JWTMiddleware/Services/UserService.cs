using JWTMiddleware.Model;

namespace JWTMiddleware.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>() { 
            new User{ 
                Id = 1,
                FirstName = "myTest",
                LastName = "User1",
                Roles = new List<Role>{Role.Customer},
                UserName = "mytestuser",
                Password = "test123"
                
            },
            new User{ 
                Id=2,
                FirstName = "myTest2",
                LastName = "User2",
                Roles= new List<Role>{Role.Admin},
                UserName = "test",
                Password = "test"
            }
        };
        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }
    }
}
