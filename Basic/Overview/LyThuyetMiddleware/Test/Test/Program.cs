
namespace Test
{
    /*Lý thuyết 
     - Middleware trong ASP.NET Core được sắp xếp thành một hàng đợi nối tiếp nhau (gọi là Request Pipeline).
     - Mỗi Middleware là một đoạn code xử lý nhỏ có 2 nhiệm vụ chính:
        + Xử lý Request đi vào: Kiểm tra, sửa đổi request (ví dụ: check token, ghi log, nén dữ liệu...).
        + Quyết định: Gọi next() để chuyển Request cho Middleware tiếp theo. Hoặc Ngắt luồng (Short-circuit): 
          Dừng ngay tại đó và trả Response về luôn (ví dụ: Phát hiện chưa đăng nhập -> trả về lỗi 401 Unauthorized ngay lập tức mà không cần gọi tiếp vào Controller)
    - Cách khai báo Middleware trong code
        + Khi khởi tạo project ASP.NET Core, các lệnh bắt đầu bằng app.Use...() chính là việc đang xếp hàng cho các Middleware:
        Ví dụ 
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            1. Middleware bắt lỗi ngoại lệ (Exception Handling)
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            2. Middleware tự động chuyển HTTP sang HTTPS
            app.UseHttpsRedirection();

            3. Middleware phục vụ file tĩnh (html, css, image trong wwwroot)
            app.UseStaticFiles();

            4. Middleware xác thực người dùng (Authentication)
            app.UseAuthentication();

            5. Middleware phân quyền (Authorization)
            app.UseAuthorization();

            6. Middleware định tuyến đến Controller
            app.MapControllers();

            app.Run();

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
