
namespace HttpLogging
{
    /*Lý thuyết 
     - Ghi log Http là một phần mềm middleware ghi lại thông tin về các Http request và response, các log Http cung cấp về
        + Request Information: URL path, HTTP Method (GET, POST...), Query String, Protocol (HTTP/2...).
        + Headers: Request Headers & Response Headers (ví dụ: User-Agent, Content-Type, Authorization...).
        + RequestBody & ResponseBody: Nội dung JSON/Text được gửi lên hoặc trả về
        + Response Status Code: 200 OK, 404 Not Found, 500 Internal Server Error
        + Execution Time: Thời gian API xử lý mất bao nhiêu miligiây (ms).
     - Việc ghi log HTTP được kích hoạt bằng cách gọi các hàm AddHttpLogging() và UseHttpLogging() 
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
            // BƯỚC 1: Đăng ký dịch vụ HttpLogging
            builder.Services.AddHttpLogging(logging =>
            {
                // Chọn những thông tin muốn ghi log
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod
                                      | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPath
                                      | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseStatusCode
                                      | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody;
                // Giới hạn độ dài Body được log để tránh ngốn RAM
                logging.RequestBodyLogLimit = 4096; // 4 KB
                logging.ResponseBodyLogLimit = 4096;
            });
            var app = builder.Build();

            // BƯỚC 2: Kích hoạt Middleware (Nên đặt sớm trong pipeline)
            app.UseHttpLogging();
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
