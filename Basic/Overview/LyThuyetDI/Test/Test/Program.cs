
namespace Test
{
    /*Lý thuyết
     - ASP .NET Core tích hợp sẵn tính năng Dependency Injection giúp các service cấu hình có thể sử dụng trong toàn bộ
    ứng dụng. Các Service này được thêm vào vùng chứa DI bằng cách sử dụng WebApplication.CreateBuilder(args) sẽ tạo ra
    một container WebApplicationBuilder. 
        + Bên trong builder này có một thuộc tính cực kỳ quan trọng là builder.Services, có kiểu dữ liệu là IServiceCollection.
        + IServiceCollection này chính là nơi "đăng ký" (register) danh sách các blueprint (bản thiết kế) cho ứng dụng: Service nào sẽ được tạo ra, và nó sống trong bao lâu.
    - Sau khi tạo ra vùng chứa Inversion of Control sử dụng builder.Services. chấm đến các phụ thuộc để có thể tiêm vào 
    ứng dụng để chạy
    - Khi chạy dòng này var app = builder.Build() IoC Container thực sự (gọi là IServiceProvider) mới được "đóng gói" và chính thức đi vào hoạt động.
    - Các cách đăng ký Service vào Container: Ngoài các hàm đăng ký có sẵn của framework như AddControllers(), AddDbContext(), 
    khi bạn tự tạo các Service của riêng mình, bạn sẽ đăng ký chúng vào builder.Services thông qua 3 Vòng đời (Lifetimes) chính:
        + Transient: Mỗi lần xin là tạo mới hoàn toàn 
            Ví dụ: builder.Services.AddTransient<IMyService, MyService>();
        + Scoped: Tạo mới 1 lần cho MỖI HTTP REQUEST (phổ biến nhất cho Web/DB)
            Ví dụ: builder.Services.AddScoped<IUserService, UserService>();
        + Singleton: Tạo đúng 1 lần duy nhất trong suốt vòng đời ứng dụng
            Ví dụ: builder.Services.AddSingleton<ICacheService, CacheService>();
    - Cách sử dụng (Inject) vào Controller: Sau khi đã đăng ký vào Container, bất kỳ đâu trong ứng dụng (ví dụ trong một Controller), chỉ cần "tiêm" nó ở Constructor:
     Ví dụ     Container sẽ tự tìm xem IUserService là gì, 
               khởi tạo nó rồi "tiêm" (inject) vào đây cho bạn dùng!
                public UserController(IUserService userService)
                {
                    _userService = userService;
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
