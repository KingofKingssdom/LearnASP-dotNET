# Các phương thức và thuộc tính Fluent API
- Các phương thức quan trọng cần nắm trong cấu hình 
fluent API thông qua phương thức OnModelCreating trong
DbContext

## Cấu hình cho bảng và enttiy
- ToTable("TableName","SchemaName"): Chỉ định tên bảng và schema tương ứng trong cơ sở dữ liệu
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // ToTable: Đổi tên bảng và schema trong DB
    modelBuilder.Entity<Product>().ToTable("tbl_Products", "store");
}
```
- HasKey(e => e.Id): Chỉ định Khóa chính (Primary Key)
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{

    // HasKey (Đơn): Đặt Id làm Khóa chính
    modelBuilder.Entity<Product>().HasKey(p => p.Id);
    // Có thể đặt tên cột 
    modelBuilder.Entity<Product>().HasKey(p => p.Name);
}
```
- HasKey(e => new { e.Id1, e.Id2 }): Khai báo Khóa chính tổ hợp (Composite Key)
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{

    // HasKey (Đơn): Đặt Id làm Khóa chính
    modelBuilder.Entity<StudentEnrollment>().HasKey(s => new{s.StudentId, s.EnrollmentId});
    
}
```
- HasIndex(e => e.Email): Tạo Index cho cột để tối ưu truy vấn.
.IsUnique(): Bắt buộc giá trị trong Index phải là duy nhất.
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
 modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();
}
```
- Ignore(e => e.Property): Bỏ qua một thuộc tính trong C#, không ánh xạ thành cột trong Database.
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
     Ignore: Bỏ qua thuộc tính TempData trong C#, KHÔNG tạo cột trong DB
    modelBuilder.Entity<Product>().Ignore(p => p.TempData);

}
```
- Ignore<EntityType>(): Bỏ qua toàn bộ lớp Entity, không tạo bảng tương ứng.
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
     Ignore: Bỏ qua thuộc tính TempData trong C#, KHÔNG tạo cột trong DB
    modelBuilder.Ignore<Product>();
}
```
## Cấu hình Cấu hình Cột & Thuộc tính
- HasColumnName("ColumnName"): Đổi tên cột trong Database khác với tên thuộc tính C#
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Name).HasColumnName("product_name");
}
```
- HasColumnType("varchar(100)"): Chỉ định chính xác kiểu dữ liệu SQL trong Database
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Description).HasColumnType("nvarchar(max)");
}
```
- IsRequired() / IsRequired(false): Đánh dấu cột là NOT NULL (bắt buộc) hoặc NULL (cho phép rỗng)
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Name).IsRequired());
}
```
- HasMaxLength(number): Quy định độ dài tối đa của chuỗi (string) hoặc mảng byte
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Name).HasMaxLength(200));
}
```
- HasPrecision(precision, scale): Quy định độ chính xác cho kiểu số thực/tiền tệ (ví dụ HasPrecision(18, 2))
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Price).HasPrecision(18, 2));
}
```
- HasDefaultValue(value): Thiết lập giá trị mặc định cố định cho cột.
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Product>(entity =>
    entity.Property(p => p.Views).HasDefaultValue(0));
}
```
## Cấu hình mối quan hệ
### A. Khai báo đầu quan hệ
- HasOne(e => e.Parent): Thực thể hiện tại có 1 đối tượng liên kết
- HasMany(e => e.Children): Thực thể hiện tại có nhiều đối tượng liên kết
### B. Khai báo đuôi quan hệ
- WithOne(p => p.Child): ...và đối tượng kia liên kết ngược lại 1 đối tượng.
- WithMany(p => p.Children): ...và đối tượng kia liên kết ngược lại nhiều đối tượng
### C. Chi tiết mối quan hệ
- HasForeignKey(e => e.ParentId): Chỉ định rõ thuộc tính làm Khóa ngoại (Foreign Key).
- HasPrincipalKey(p => p.CustomKey): Chỉ định cột khóa chính bên bảng chính (nếu không dùng PK mặc định)
- OnDelete(DeleteBehavior...): Định nghĩa hành vi khi xóa bản ghi cha
    + DeleteBehavior.Cascade: Xóa liên hoàn các bản ghi con
    + DeleteBehavior.Restrict: Chặn xóa nếu còn bản ghi con
    + DeleteBehavior.SetNull: Gán FK của bản ghi con thành NULL
- UsingEntity("JoinTableName"): Tùy chỉnh tên bảng trung gian trong quan hệ Nhiều - Nhiều (Many-to-Many)
