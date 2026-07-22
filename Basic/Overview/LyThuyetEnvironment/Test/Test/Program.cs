
namespace Test
{
    /*Lý thuyết
     - Environment giúp ứng dụng biết nó đang được chạy ở đâu, từ đó tự động điều chỉnh hành vi và cấu hình tương ứng.
     - Môi trường trong ASP.NET Core chỉ đơn giản là một chuỗi văn bản (string) được đọc từ biến môi trường của hệ thống (OS/Server/Container) có tên là
            ASPNETCORE_ENVIRONMENT
     - Mặc dù bạn có thể đặt tên môi trường là bất kỳ chữ gì, ASP.NET Core có 3 tên môi trường chuẩn được dựng sẵn:
        + Development (Phát triển): Chạy dưới máy cá nhân của lập trình viên (Local PC).
        + Staging (Kiểm thử): Chạy trên server thử nghiệm (QA/UAT) để kiểm tra tính năng trước khi ra mắt.
        + Production (Sản xuất/Thực tế): Chạy trên server thật cung cấp dịch vụ cho người dùng cuối.
     - Khi app vừa bật lên, Host sẽ tìm biến môi trường ASPNETCORE_ENVIRONMENT.
            Khi chạy từ Visual Studio / VS Code / Rider:
            Giá trị này được cấu hình sẵn trong file Properties/launchSettings.json
     - Biến Environment tác động lên 2 nơi quan trọng nhất trong app:
       + Tự động chuyển đổi File Cấu hình (appsettings.json): ASP.NET Core sẽ nạp file cấu hình theo quy tắc "ghi đè" (override):
                => Đọc file chung appsettings.json.
                => Đọc tiếp file riêng appsettings.{Environment}.json tương ứng với môi trường hiện tại và ghi đè các thông số trùng lặp.
       Ví dụ thực tế:
                appsettings.Development.json cài đặt chuỗi kết nối Database local: Server=localhost;Database=DevDb;...
                appsettings.Production.json cài đặt chuỗi kết nối Database thật: Server=10.0.0.5;Database=ProdDb;...
       + Thay đổi hành vi Code trong Program.cs hoặc Controller
                => Sử dụng trực tiếp trong Program.cs
            Ví dụ: 
                    var builder = WebApplication.CreateBuilder(args);
                    var app = builder.Build();

                    // Lấy đối tượng Environment
                    IWebHostEnvironment env = app.Environment;

                    if (env.IsDevelopment())
                    {
                        // Chỉ bật các công cụ hiển thị lỗi chi tiết & Swagger khi code dưới máy Dev
                        app.UseDeveloperExceptionPage();
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    }
                    else
                    {
                        // Khi lên Production: Ẩn thông tin lỗi chi tiết để bảo mật, dùng trang báo lỗi chung
                        app.UseExceptionHandler("/Home/Error");
                        app.UseHsts();
                    }
                => Inject IWebHostEnvironment vào Controller hoặc Service
            Ví dụ: 
                public class HomeController : ControllerBase
                {
                    private readonly IWebHostEnvironment _env;

                    public HomeController(IWebHostEnvironment env)
                    {
                        _env = env;
                    }

                    [HttpGet("info")]
                    public IActionResult GetEnvironmentInfo()
                    {
                        // 1. Kiểm tra các hàm có sẵn
                        if (_env.IsDevelopment())  // Logic riêng cho Dev  
                        if (_env.IsProduction()) // Logic riêng cho Prod 

                        // 2. Kiểm tra môi trường tự định nghĩa (Custom Environment)
                        if (_env.IsEnvironment("Testing")) // Logic riêng cho mảng Test

                        // 3. Lấy đường dẫn thư mục gốc của dự án (WebRootPath / ContentRootPath)
                        string wwwrootPath = _env.WebRootPath;

                        return Ok(new { 
                            EnvironmentName = _env.EnvironmentName,
                            RootPath = _env.ContentRootPath 
                        });
                    }
                }
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
