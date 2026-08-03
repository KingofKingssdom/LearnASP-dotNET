using Authen.Context;
using Microsoft.EntityFrameworkCore;

namespace Authen.Properties.Extension
{
    public static class DatabaseExtention
    {
        public static IServiceCollection AddDatabase(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
