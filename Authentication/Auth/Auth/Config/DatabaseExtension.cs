using Auth.Context;
using Microsoft.EntityFrameworkCore;

namespace Auth.Config
{
    public static class DatabaseExtension
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
