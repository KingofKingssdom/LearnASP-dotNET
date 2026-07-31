# Các thuộc tính của Entity
## I. Các thuộc tính bao gồm
+ Theo quy ước , tất cả các thuộc tính công khai có phương thức getter và setter sẽ được bao gồm trong model
+ Các thuộc tính cụ thể có thể bị loại trừ như sau:
	+ Sử dụng Annotation
	``` C#
	public class Blog
	{
		public int BlogId { get; set; }
		public string Url { get; set; }

    [NotMapped]
    public DateTime LoadedFromDatabase { get; set; }
	}
	```
	+ Sử dụng API Fluent
	``` C#
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Blog>()
			.Ignore(b => b.LoadedFromDatabase);
	}
	```
## II. Đổi tên cột
+ Theo quy ước, khi sử dụng cơ sở dữ liệu quan hệ, các thuộc tính của thực thể được ánh xạ tới các cột trong bảng có cùng tên với thuộc tính đó.
+ Có thể đổi tên
	+ Sử dụng Annotation
	``` C#
	public class Blog
	{
		[Column("blog_id")]
		public int BlogId { get; set; }

		public string Url { get; set; }
	}
	```
	+ Sử dụng API Fluent 
	``` C#
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Blog>()
			.Property(b => b.BlogId)
			.HasColumnName("blog_id");
	}
	```
## III. Kiểu dữ liệu cột
+ Theo mặc định thì mỗi cột trong database sẽ tự động match với các kiểu dữ liệu trong C# tùy vào loại database
+ Có thể chỉnh sửa và  thay đổi kiểu dữ liệu trong database qua 2 cách
	+ Sử dụng Annotation
	``` C#
	public class Blog
	{
		public int BlogId { get; set; }

		[Column(TypeName = "varchar(200)")]
		public string Url { get; set; }

		[Column(TypeName = "decimal(5, 2)")]
		public decimal Rating { get; set; }
	}
	```
	+ Sử dụng API Fluent
	
	```C#
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
    modelBuilder.Entity<Blog>(
        eb =>
        {
            eb.Property(b => b.Url).HasColumnType("varchar(200)");
            eb.Property(b => b.Rating).HasColumnType("decimal(5, 2)");
        });
	}
	```
## IV. Chiều dài lưu trữ của của dư liệu
Có thể điều chỉnh chiều dài lưu trữ dữ liệu
+ Dùng Annotation
``` C#
public class Blog
{
    public int BlogId { get; set; }

    [MaxLength(500)]
    public string Url { get; set; }
}
```
+ Dùng API Fluent
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .Property(b => b.Url)
        .HasMaxLength(500);
}
```
## V. Đặt độ chính xác đấu phẩy động
+ Kiểu dữ liệu như decimal hoặc kiểu dữ liệu datetime có thể có chứa dữ liệu nhiều dấu phẩy phía sau
+ Có thể chỉnh sử kiểu số chính xác sau dấu phẩy động
	+ Sử dụng Annotation
	``` C#
	public class Blog
	{
		public int BlogId { get; set; }
		[Precision(14, 2)]
		public decimal Score { get; set; }
		[Precision(3)]
		public DateTime LastUpdated { get; set; }
	}
	```
	+ Sử dụng API Fluent
	``` C#
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
    modelBuilder.Entity<Blog>()
        .Property(b => b.Score)
        .HasPrecision(14, 2);

    modelBuilder.Entity<Blog>()
        .Property(b => b.LastUpdated)
        .HasPrecision(3);
	}
	```
## VI. Thuộc tính bắt buộc và thuộc tính tùy chọn
+ Thuộc tính bắt buộc là thuộc tính không cho phép giá trị null
+ Thuộc tính tùy chọn là thuộc tính cho phép giá trị null

Theo quy ước trong .NET các thuộc tính int, bool, string, decimal
không có dấu ? phía sau thì nó là thuộc tính bắt buộc. Còn nếu muốn 
có thuộc tính tùy chọn thì chỉ cần thêm ? sau như int?, bool?, string?
<br/>
Với kiểu dữ liệu là tham chiếu thì có 2 cách đặt kiểu dữ liệu bắt buộc
+ Sử dụng bình thường không có dấu ? thì nó là thuộc tính bắt buộc
	``` C#
	public class Customer
	{
		public int Id { get; set; }
		public string FirstName { get; set; } // Required by convention
		public string LastName { get; set; } // Required by convention
		public string? MiddleName { get; set; } // Optional by convention
	```
+ Sử dụng Annotation
	``` C#
	public class CustomerWithoutNullableReferenceTypes
	{
		public int Id { get; set; }

		[Required] // Data annotations needed to configure as required
		public string FirstName { get; set; }

		[Required] // Data annotations needed to configure as required
		public string LastName { get; set; }

		public string MiddleName { get; set; } // Optional by convention
	}
	```
+ Sử dụng API Fluent
	``` C#
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Blog>()
			.Property(b => b.Url)
			.IsRequired();
	}
	```
