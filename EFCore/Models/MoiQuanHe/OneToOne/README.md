# Cấu hình mỗi quan hệ One to One
## Cách 1 cấu hình sử dụng Navigation Property
``` C#
public class User
{
    public int Id { get; set; }
    public UserProfile? Profile { get; set; } // Navigation property
}

public class UserProfile
{
    public int Id { get; set; }
    public int UserId { get; set; } // Khóa ngoại (FK) tự nhận diện
    public User User { get; set; } = null!; // Navigation property
}

```
## Cách 2 sử dụng fluent API
Trong nhiều trường hợp, quy ước tự động không thể xác định bên nào là Principal, bên nào là Dependent. Sử dụng Fluent API trong OnModelCreating giúp cấu hình rõ ràng nhất:
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>()
        .HasOne(u => u.Profile)
        .WithOne(p => p.User)
        // BẮT BUỘC: Chỉ định rõ lớp nào chứa FK bằng HasForeignKey<T>
        .HasForeignKey<UserProfile>(p => p.UserId);
}
```
Lưu ý quan trọng: Khi dùng .WithOne(), bạn bắt buộc phải truyền kiểu dữ liệu của Dependent Entity vào HasForeignKey<UserProfile>(...) để EF Core biết chính xác bảng nào sẽ tạo cột FK.


