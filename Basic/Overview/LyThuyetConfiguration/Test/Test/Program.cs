
namespace Test
{
    /*Lý thuyết
     - Configuration (Cấu hình) trong ASP.NET Core đơn giản là nơi lưu giữ các thông tin cài đặt của ứng dụng mà bạn không muốn viết chết (hard-code) vào trong code C#
    hiện tại mà tách riêng ra một file chỉ chuyên chứa các cấu hình cài đặt để chạy ứng dụng 
    - Configuration Providers: Điểm cực kỳ xịn của ASP.NET Core là nó cho phép gom cấu hình từ nhiều nguồn khác nhau lại thành một mối duy nhất.
       => Khi bạn chạy dòng var builder = WebApplication.CreateBuilder(args);, ASP.NET Core tự động nạp cấu hình theo thứ tự ưu tiên (cái sau ghi đè cái trước):
        + File appsettings.json: File cấu hình mặc định phổ biến nhất.
        + File appsettings.{Environment}.json: File cấu hình riêng cho từng môi trường (ví dụ: appsettings.Development.json hay appsettings.Production.json).
        + User Secrets: Nơi lưu cấu hình bảo mật (như password DB, API Key) ở máy cá nhân khi lập trình, tránh lỡ tay push lên GitHub.
        + Biến môi trường (Environment Variables): Thường dùng khi deploy app lên Server/Docker/Kubernetes.
        + Tham số dòng lệnh (Command-line arguments): Truyền trực tiếp khi gõ lệnh chạy app.
    - File cấu hình sẽ là file appsettings.json. Tuy nhiên có nhiều nguồn khác nhau để đặt cài đặt cấu hình

    - Cách lấy giá trị Configuration ra sử dụng
        + Cách 1 đọc trực tiếp qua IConfiguration (Cách nhanh)
            Bất kỳ đâu có IConfiguration (đã được DI Container tự động đăng ký sẵn), bạn có thể Inject vào và gọi:
            Ví dụ 
            public class HomeController : ControllerBase
            {
                private readonly IConfiguration _configuration;

                public HomeController(IConfiguration configuration)
                {
                    _configuration = configuration;
                }

                [HttpGet("connection")]
                public IActionResult GetConnection()
                {
                    // 1. Lấy chuỗi kết nối bằng hàm có sẵn
                    string dbConn = _configuration.GetConnectionString("DefaultConnection");

                    // 2. Lấy giá trị theo đường dẫn dấu hai chấm ":"
                    string jwtSecret = _configuration["MyJwtSettings:Secret"];

                    return Ok(new { dbConn, jwtSecret });
                    }
                }
         + Cách 2: Options Pattern (Cách chuẩn nhất - Best Practice)
                Thay vì đọc từng chuỗi thủ công dễ gõ sai chính tả, nên ánh xạ (map) một nhóm cấu hình vào thẳng một Class C#.
                Ví dụ 
                Bước 1: Tạo Class tương ứng với file JSON
                public class JwtSettings
                {
                    public string Secret { get; set; } = string.Empty;
                    public int ExpirationInMinutes { get; set; }
                 }
                Bước 2: Đăng ký trong Program.cs
                // Đánh ánh xạ nhánh "MyJwtSettings" trong appsettings.json vào Class JwtSettings
                builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("MyJwtSettings")
                );
                Bước 3: Inject vào Controller để dùng dưới dạng strongly-typed (không lo gõ sai chữ)
                public class AuthController : ControllerBase
                {
                    private readonly JwtSettings _jwtSettings;

                    // Inject IOptions<JwtSettings>
                    public AuthController(IOptions<JwtSettings> jwtOptions)
                    {
                        _jwtSettings = jwtOptions.Value;
                    }

                    [HttpGet]
                    public IActionResult GetExpire()
                    {
                        // Có sẵn nhắc code int, string rõ ràng!
                        int minutes = _jwtSettings.ExpirationInMinutes; 
                        return Ok(minutes);
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
