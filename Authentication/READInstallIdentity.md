# Các triển khai Identity trong ASP .NET Core
+ Bước 1: Tạo dự án
+ Bước 2: Thêm gói NuGet
**Microsoft.AspNetCore.Identity.EntityFrameworkCore:**  Gói này tích hợp ASP.NET Core Identity với Entity Framework Core. Nó cung cấp các chức năng cần thiết để lưu trữ người dùng, vai trò và các dữ liệu liên quan đến định danh khác trong cơ sở dữ liệu bằng EF Core. 
+ Bước 3: <br/>
    + Lớp IdentityDbContext trong ASP.NET Core Identity đóng vai trò thiết yếu trong việc tích hợp hệ thống quản lý thành viên Identity với Entity Framework Core. Lớp IdentityDbContext này kế thừa từ DbContext và được thiết kế để quản lý ngữ cảnh cơ sở dữ liệu cho dữ liệu định danh. 
    + Lớp Context của ứng dụng chúng ta cần kế thừa từ  lớp IdentityDbContext  thay vì  lớp DbContext  . Điều này là do  IdentityDbContext  kế thừa từ lớp DbContext của Entity Framework Core và cung cấp tất cả  các thuộc tính DbSet  cần thiết để quản lý các bảng Identity.

    ``` C#
    public class ApplicationDbContext : IdentityDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }
        }
    ```
+ Bước 4: Đăng ký Identity Service trong ASP.NET Core
chúng ta cần cấu hình các dịch vụ ASP.NET Core Identity trong bộ chứa DI. Điều này cho phép quản lý người dùng và vai trò cùng với các tính năng Identity mặc định như băm và xác thực mật khẩu.

    ```C#
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();
    ```

IdentityUser và IdentityRole đại diện cho các thực thể người dùng và vai trò mặc định. Phương thức AddEntityFrameworkStores<ApplicationDbContext>() hướng dẫn Identity sử dụng ngữ cảnh EF Core (ApplicationDbContext) để lưu trữ dữ liệu người dùng và vai trò.

**Middleware dùng Authentication và Authorization**
Sau khi đăng ký các dịch vụ, hãy thêm các thành phần middleware vào quy trình xử lý yêu cầu HTTP để kích hoạt khả năng xác thực và ủy quyền:

+ app.UseAuthentication(); // Cho phép hệ thống xác thực kiểm tra thông tin đăng nhập của người dùng
+ app.UseAuthorization(); // Cho phép kiểm tra quyền truy cập dựa trên vai trò hoặc chính sách

Lưu ý quan trọng: Hãy đặt các lệnh gọi middleware này sau middleware định tuyến (app.UseRouting()) nhưng trước middleware định tuyến MVC (app.MapControllerRoute(…)) để hệ thống Identity có thể xác thực và ủy quyền người dùng trước khi các yêu cầu đến bộ điều khiển của bạn.

**Tại sao lại theo thứ tự này?**
+ Middleware app.UseRouting() sẽ khớp yêu cầu đến với một điểm cuối cụ thể (ví dụ: một hành động của bộ điều khiển).
+ Quá trình xác thực (app.UseAuthentication()) cần diễn ra sau khi định tuyến vì nó sử dụng thông tin về điểm cuối và yêu cầu để xác thực người dùng một cách chính xác.
+ Quá trình ủy quyền (app.UseAuthorization()) chạy sau khi xác thực và sử dụng thông tin người dùng đã được xác thực để kiểm tra xem quyền truy cập vào điểm cuối được yêu cầu có được cho phép dựa trên vai trò, chính sách hoặc yêu cầu hay không.
+ Cuối cùng, app.MapControllerRoute(…) chỉ thực thi điểm cuối phù hợp (hành động của bộ điều khiển) sau khi quá trình xác thực và ủy quyền hoàn tất thành công.