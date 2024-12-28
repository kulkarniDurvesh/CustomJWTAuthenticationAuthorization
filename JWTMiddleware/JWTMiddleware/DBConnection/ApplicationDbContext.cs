using JWTMiddleware.Model;
using Microsoft.EntityFrameworkCore;

namespace JWTMiddleware.DBConnection
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContext) : base(dbContext)
        {
            //_context = dbContext;
        }

        public DbSet<User> Users { get; set; }
    }
}
