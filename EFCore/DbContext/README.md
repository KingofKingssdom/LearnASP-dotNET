# Hiểu về DBContext
## I. DbContext là gì và làm nhiệm vụ gì ?
Về mặt kỹ thuật, DbContext đại diện cho một phiên làm việc (Session) làm việc với Database. Nó chịu trách nhiệm cho các công việc chính:
+ Quản lý kết nối (Connection Management): Mở và đóng kết nối tới CSDL khi cần
+ Ánh xạ Entity - Database (Mapping): Chuyển đổi các lớp C# thành các Bảng (Tables) trong CSDL và ngược lại.
+ Theo dõi sự thay đổi (Change Tracking): Ghi nhớ các thay đổi trên Entity (thêm mới, cập nhật, xóa) để chuẩn bị lưu xuống CSDL.
+ Truy vấn dữ liệu (Querying): Biến các câu truy vấn LINQ trong C# thành câu lệnh SQL tương ứng để thực thi dưới Database.
+ Lưu dữ liệu (Persistence): Thực thi lệnh SaveChangesAsync() / SaveChanges() để gom tất cả thay đổi thành các câu lệnh SQL INSERT, UPDATE, DELETE chạy trong một Transaction.

## II. Cấu trúc cơ bản của một lớp DbContext
Để tạo một DbContext, bạn tạo một class kế thừa từ DbContext.

```C#
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // 1. Constructor nhận DbContextOptions để cấu hình (Chuỗi kết nối, provider SQL/SQLite,...)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 2. Các DbSet tương ứng với các Bảng trong Database
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }

    // 3. Cấu hình chi tiết các mối quan hệ, ràng buộc (Fluent API)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ví dụ: Cấu hình khóa chính, độ dài chuỗi, mối quan hệ N-N...
        modelBuilder.Entity<Brand>()
            .Property(b => b.BrandCode)
            .IsRequired()
            .HasMaxLength(20);
    }
}
```
## III. Các thành phần quan trọng bên trong DbContext
A. DbSet`<TEntity>`
+ Mỗi DbSet`<TEntity>` đại diện cho một bảng trong CSDL hoặc một tập hợp các đối tượng cùng loại.
+ Dùng DbSet để thực hiện truy vấn LINQ:
ví dụ: 
``` C#
public DbSet<Product> Products { get; set; }

context.Products.Where(p => p.Price > 100).ToList()
```

B. OnConfiguring vs DbContextOptions

Có 2 cách truyền chuỗi kết nối (ConnectionString) và cấu hình Database Provider (SqlServer, Sqlite, PostgreSql...):
 + Cách 1 (Dùng cho Console / App nhỏ): Ghi đè phương thức OnConfiguring:
 
 ``` C# 
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseSqlServer("Server=...;Database=...;");
}
 ```

 + Cách 2 (Dùng trong ASP.NET Core - Standard): Đăng ký qua Dependency Injection (DI) trong file Program.cs:

 ``` C#
 builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 ```

 C. OnModelCreating (Fluent API)
Nơi bạn định nghĩa các quy tắc phức tạp mà thuộc tính (Attributes) trên Model không làm được:
+ Mối quan hệ 1-1, 1-N, N-N.
+ Tạo chỉ mục (Index), Giá trị mặc định (Default Value).
+ Data Seeding (Tạo dữ liệu mẫu ban đầu).

## 4. Cơ chế hoạt động cốt lõi: Change Tracker (Theo dõi thay đổi)
Đây là điểm ăn tiền nhất của DbContext. Mỗi Entity khi được load từ DB ra hoặc gán vào DbContext sẽ được gán cho một trạng thái (EntityState):
+ Unchanged: Dữ liệu giống hệt dưới DB, chưa sửa gì.
+ Added: Đối tượng mới thêm vào, sẽ chạy lệnh INSERT khi SaveChanges().
+ Modified: Đối tượng đã bị sửa thuộc tính, sẽ chạy lệnh UPDATE khi SaveChanges().
+ Deleted: Đối tượng đánh dấu xóa, sẽ chạy lệnh DELETE khi SaveChanges().
+ Detached: Đối tượng không được DbContext quản lý/theo dõi.

``` C#
// 1. Fetch từ DB -> EntityState = Unchanged
var product = await context.Products.FindAsync(1);

// 2. Thay đổi thuộc tính -> EF Core tự đổi EntityState = Modified
product.Price = 150;

// 3. Thực thi -> EF Core kiểm tra Tracker, thấy "Modified" nên phát ra câu lệnh UPDATE SQL
await context.SaveChangesAsync();

```

## 5. Vòng đời (Lifetime) của DbContext trong ASP.NET Core
Trong ứng dụng Web (ASP.NET Core), DbContext mặc định được đăng ký với Scope là Scoped.
Scoped có nghĩa là gì?
+ Với mỗi HTTP Request gửi tới server, 1 instance (thể hiện) của DbContext sẽ được tạo ra.
+ Toàn bộ các Controller / Service xử lý request đó sẽ dùng chung instance DbContext này.
+ Khi HTTP Request kết thúc (trả kết quả về cho client), DbContext sẽ tự động bị tiêu hủy (Dispose) và giải phóng kết nối DB.
