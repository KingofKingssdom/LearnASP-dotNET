# Connect string trong EF Core
## I. Khái niệm
Connection String (Chuỗi kết nối) trong EF Core là một chuỗi ký tự chứa toàn bộ "thông tin định danh và tọa độ" cần thiết để ứng dụng C# có thể tìm thấy, đăng nhập và tương tác với cơ sở dữ liệu (Database)
## II. Một Connect string gồm những thông tin gì
Connection String (Chuỗi kết nối) trong EF Core là một chuỗi ký tự chứa toàn bộ "thông tin định danh và tọa độ" cần thiết để ứng dụng C# có thể tìm thấy, đăng nhập và tương tác với cơ sở dữ liệu (Database)
+ Server / Data Source (Máy chủ DB): Nơi chứa database (địa chỉ IP, domain, tên máy chủ local hoặc tên instance).
+ Database / Initial Catalog (Tên CSDL): Tên của database cụ thể mà bạn muốn làm việc.
+ Authentication / Security (Xác thực thanh danh):
	+ Windows Authentication (Xác thực Windows): Dùng tài khoản Windows đang đăng nhập (ví dụ: Trusted_Connection=True; hoặc Integrated Security=True;)
	+ SQL Authentication (Tài khoản SQL): Dùng Username và Password (ví dụ: User Id=sa;Password=YourPassword;)
+ Additional Options (Các tùy chọn bổ sung): </br>
Ví dụ TrustServerCertificate=True; (bỏ qua kiểm tra chứng chỉ SSL khi dev local), Encrypt=True;, MultipleActiveResultSets=True; (MARS - cho phép nhiều truy vấn đồng thời trên 1 kết nối).
<br/>
Ví dụ
- SQL Server LocalDB
``` C#
Server=(localdb)\mssqllocaldb;Database=MyStoreDb;Trusted_Connection=True;TrustServerCertificate=True;
```
- SQL Server thực tế (Dùng User & Password):
``` C#
Server=192.168.1.10;Database=MyStoreDb;User Id=sa;Password=SuperSecret123!;TrustServerCertificate=True;
```
## III. Cách quản lý Connect string chuẩn trong ASP .Net Core
- Bước 1: Lưu trong appsettings.json </br>
Đưa chuỗi kết nối vào phần "ConnectionStrings" của file cấu hình:
``` C#
{
  "Logging": { ... },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MyStoreDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
- Bước 2: Đọc và truyền vào DbContext tại Program.cs </br>
Trong Program.cs, sử dụng builder.Configuration.GetConnectionString("DefaultConnection") để lấy chuỗi này ra và đăng ký vào DI
``` C#
var builder = WebApplication.CreateBuilder(args);

// 1. Đọc Connection String từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Không tìm thấy Connection String 'DefaultConnection'.");

// 2. Đăng ký DbContext với Provider (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
```
- Bước 3: Lớp DbContext tiếp nhận cấu hình </br>
Lớp AppDbContext chỉ cần nhận DbContextOptions qua Constructor và truyền xuống lớp cơ sở base:
```C#
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}
```

