using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Context
{   
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { set; get; }

    }
}
