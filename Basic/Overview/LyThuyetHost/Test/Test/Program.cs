
namespace Test
{
    /*Lý thuyết
     - Host là chiếc hộp gói gọn tất cả mọi thứ (Server, DI, Middleware, Config) lại để ứng dụng web của có thể bật lên
    và chạy được, và từ .NET 6 trở đi bạn cứ yên tâm dùng WebApplication (Minimal Host) là chuẩn nhất!
    - Khi ứng dụng ASP.NET Core khởi động, Host sẽ đứng ra gom tất cả các thành phần cốt lõi này lại:
        + An HTTP server implementation: Động cơ nhận web request (thường là Kestrel Server).
        + Middleware components: Các cửa kiểm soát an ninh để lọc request/response.
        + Logging: Hệ thống ghi lại nhật ký hoạt động/lỗi của app.
        + Dependency injection (DI) services: Kho quản lý các dịch vụ mà bạn đăng ký qua builder.Services.
        + Configuration: Nơi đọc file cấu hình (như appsettings.json, biến môi trường...).
    => Nếu không có Host, các dịch vụ như DI, Middleware, Server... chỉ là những mảnh ghép rời rạc và không thể tự hoạt động cùng nhau.
    - Có 3 kiểu Host
        + WebApplication / WebApplicationBuilder (Minimal Host): Loại mới nhất (từ .NET 6 trở đi). Dùng WebApplication.CreateBuilder(args) chính là dùng loại này. Code rất ngắn gọn, dễ đọc.
        + .NET Generic Host (Generic Host): Loại dùng chung cho cả Web, App chạy ngầm (Background Service), Console App. Viết phức tạp hơn loại 1 một chút vì cần nhiều hàm Callback.
        + ASP.NET Core WebHost (Web Host): Loại đời đầu (Thời .NET Core 1.x / 2.x). Giờ chỉ giữ lại để tương thích với các dự án siêu cũ (Backward compatibility).
    
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
