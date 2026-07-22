
namespace Test
{
    /*Lý thuyết
     - GHi log để theo dõi hành vi của ứng dụng, sử dụng log thông qua API ILogger
     - ASP .NET core sau khi gọi phương thức Webapplication.CreateBuilder(args) trong program thì nó có 4 phương thức
    để ghi log là 
        + Console
        + Debug
        + EvebtSource
        + WindowsEventLog
     - Để có thể xóa mấy cái mặc định trên thì dùng hàm builder.Logging.ClearProvider() để xóa đi và add cái nhà cung 
    cấp ghi log vào builder.Logger.AddConsole()
     - Muốn log nào ghi ra được thì log đó phải có nhãn dãn (chính là cái class được gán vào <>).
     - Dựa vào cái class đó, có thể bật/tắt hoặc chỉnh in log chi tiết hay ngắn gọn cho từng nơi trong file cấu hình appsettings.json
        Ví dụ
                {
          "Logging": {
            "LogLevel": {
              "Default": "Information",                  // Mặc định toàn app sẽ in từ mức Information trở lên
              "Microsoft.AspNetCore": "Warning",         // Riêng cái "Nhãn" Microsoft.AspNetCore chỉ in mức Warning trở lên
              "YourApp.Controllers.CounterController": "Debug" // Riêng Controller của thì in chi tiết đến tận mức Debug
            }
          }
        }
     - Mức độ ghi log càng thấp thì càng hiện thông tin chi tiết từ giá trị thấp đến giá trị cao, mức độ ghi log càng cao
    thì chỉ hiện những log cao hơn những log nhỏ hơn thì không ghi ra
        Bảng log
            Trace: 0  mục đích dùng là để ghi lại mọi biến, mọi luồng chạy nhỏ nhất
            Debug: 1  mục đích hỗ trợ sử lỗi, kiểm tra trạng thái ứng dụng
            Infomation: 2 mục đích lấy thông tin luồng chạy của ứng dụng
            Warning: 3 mục đích cảnh báo bất thường như app chưa sập nhưng sắp hết dung lượng
            Error: 4 mục đích là một tính năng hỏng không thể kết nối lỗi (database lỗi connect, exception)
            Critical: 5 Toàn bộ hệ thống sập
            None: 6
    => Việc ghi log trong cấp class là để ứng dụng chạy đến có bắn ra log loại gì, còn cấu hình trong appsetting là để 
    cấu hình xem toàn bộ ứng dụng sẽ in ra log cấp độ nào, cho dù class đó có để là thấp nhưng thg appsetting để cao thì vẫn
    chỉ in ra log cấp appsetting chỉ định
       - 
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
            app.Logger.LogWarning("App bat dau chay");

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
