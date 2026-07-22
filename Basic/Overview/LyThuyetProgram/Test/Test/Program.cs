
namespace Test
{
    /*Lý thuyết
     - File Program.cs là file đầu tiên được chạy khi khởi động ứng dụng
     - Đây là nơi chứa các cấu hình cho toàn bộ ứng dụng
     - Đây là nơi chứa các thành phần midleware
     - Trong các ứng dụng như web blazor, page Razor, Controller MVC với UI, API với controller, API minimal đều chứa các
        file program
     */
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
