# Các khóa trong EF Core
## I. Cấu hình khóa chính
+ Theo quy ước các thuộc tính được đặt tên có đuôi là Id sẽ được 
tự động cấu hình là khóa chính trong entity

    ``` C#
    public class Car
    {
        public string Id { get; set; }
    }

    public class Truck
    {
        public string TruckId { get; set; }

    }
    ```
+ Sử dụng Annotaion để cấu hình khóa chính
    ``` C#
    public class Car
    {
        [Key]
        public string LicensePlate { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
    }
    ```
+ Sử dụng API Fluent
    ``` C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => c.LicensePlate);
    }
    ```
Đổi tên khóa chính
    ``` C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasKey(b => b.BlogId)
            .HasName("PrimaryKey_BlogId");
    }
    ```

## II. Khóa tổng hợp
Cấu hình nhiều thuộc tính làm khóa của một thực thể - điều này được gọi là khóa tổng hợp. Các quy ước chỉ thiết lập khóa tổng hợp trong những trường hợp cụ thể - ví dụ như đối với một tập hợp kiểu sở hữu.
+ Sử dụng Annotation
    ``` C#
    [PrimaryKey(nameof(State), nameof(LicensePlate))]
    public class Car
    {
        public string State { get; set; }
        public string LicensePlate { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }
    }
    ```
+ Sử dụng API Fluent
    ``` C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasKey(c => new { c.State, c.LicensePlate });
    }
    ```
## III. Thuộc tính mặc định giá trị

Trong cơ sở dữ liệu quan hệ, một cột có thể được cấu hình với giá trị mặc định; nếu một hàng được chèn mà không có giá trị cho cột đó, giá trị mặc định sẽ được sử dụng.
+ Cấu hình API Fluent
    ``` C# 
    modelBuilder.Entity<Blog>()
        .Property(b => b.Rating)
        .HasDefaultValue(3);
    ```
