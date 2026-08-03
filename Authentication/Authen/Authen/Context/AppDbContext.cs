using Authen.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authen.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //public DbSet<Users> Users { set; get; }
        //public DbSet<Roles> Roles { set; get; }
        //public DbSet<UserRoles> UserRoles { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<UserRoles>()
            //.HasKey(ur => new { ur.UserId, ur.RoleId });

            //// 2. Định nghĩa mối quan hệ 1 - Nhiều từ Post sang PostTag
            //modelBuilder.Entity<UserRoles>()
            //    .HasOne(r => r.Roles)
            //    .WithMany(ur=> ur.UserRoles)
            //    .HasForeignKey(ur => ur.RoleId);

            //// 3. Định nghĩa mối quan hệ 1 - Nhiều từ Tag sang PostTag
            //modelBuilder.Entity<UserRoles>()
            //    .HasOne(u => u.Users)
            //    .WithMany(ur => ur.UserRoles)
            //    .HasForeignKey(ur => ur.UserId);
        }
    }
    }
