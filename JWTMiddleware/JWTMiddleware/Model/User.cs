using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JWTMiddleware.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public List<Role> Roles { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

    }
}
