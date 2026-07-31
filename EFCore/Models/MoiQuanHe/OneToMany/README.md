#  Tạo mối quan hệ một nhiều của Entity trong EF Core
## I. Thuộc tính bắt buộc trong mối quan hệ One To Many
Mối quan hệ một-nhiều được tạo thành từ
+ Một hoặc nhiều thuộc tính khóa chính hoặc khóa thay thế trên thực thể chính; đó là đầu "một" của mối quan hệ.<br/>
Ví dụ: Blog.Id.
+ Một hoặc nhiều thuộc tính khóa ngoại trên thực thể phụ thuộc; đó là phía "nhiều" của mối quan hệ.<br/>
Ví dụ: Post.BlogId.
+ Từ bảng này có thể chọc vào nhìn thấy dữ liệu từ bảng kia là tùy chọn thông qua điều hướng <br/>
Ví dụ: Blog.Posts.

	``` C#
	// Principal (parent)
    public class Blog
    {
        public int Id { get; set; }
        public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
    }

    // Dependent (child)
    public class Post
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // Required foreign key property
        public Blog Blog { get; set; } = null!; // Required reference navigation to principal
    }
	```
Ví dụ trên này là các giá trị của thuộc tính khóa ngoại Post.BlogId bắt buộc
phải tham chiếu đến giá trị của khóa chính bên Blog. Không được phép để giá trị null

Trường hợp các điều hướng, khóa ngoại hoặc tính chất bắt buộc/tùy chọn của mối quan hệ không được xác định theo quy ước, những điều này có thể được cấu hình rõ ràng trong API Fluent

   ``` C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Blog)
            .HasForeignKey(e => e.BlogId)
            .IsRequired();

    }
   ```
   Hoặc dùng như này 
   ``` C# 
   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    modelBuilder.Entity<Post>()
        .HasOne(e => e.Blog)
        .WithMany(e => e.Posts)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
    }
   ```
Lưu ý là chỉ cần viết một trong hai thôi là đủ

## II. Thuộc tính tùy chọn trong mối quan hệ One To Many

Với thuộc tính tùy chọn thì việc chèn vào khóa ngoại có thể cho phép khóa ngoại đó
đặt giá trị là null
``` C#
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int? BlogId { get; set; } // Optional foreign key property
    public Blog? Blog { get; set; } // Optional reference navigation to principal
}
```
Nếu không phát hiện theo quy ước, hoặc đơn giản chỉ muốn dữ liệu trong entiy
chỉ đơn thuần làm chức năng tạo thuộc tính thì sử dụng API Fluent 

``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .IsRequired(false);
}
```
### III. Điều hướng một chiều từ chính sang phụ
Điều hướng một chiều từ bảng chính có thể chọc qua đến dữ liệu bảng phụ
nhưng từ bảng phụ không thể chọc ngược về bảng chính được
``` C#
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
    public ICollection<Post> Posts { get; } = new List<Post>(); // Collection navigation containing dependents
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; } // Required foreign key property
}
```

Tương tư có thể cấu hình dựa trên API Fluent

``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany(e => e.Posts)
        .WithOne()
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```
Lưu ý tham số WithOne để trống để EF Core hiểu rắng không có điều
hướng ngược lại từ Entity phụ về Entity chính

Nếu đứng từ entity phụ có thể điều hướng đến Entity chính theo chiều 
ngược lại nhưng ý nghĩa chọc dữ liệu từ Entity phụ không chọc đến đc Entity chính
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasOne<Blog>()
        .WithMany(e => e.Posts)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```
### IV. Điều hướng một chiều từ thực thể phụ về chính
Tương tự ta có thể điều hướng dữ liệu phụ chọc vào bảng chính được
nhưng không chọc từ bảng chính qua phụ được 
``` C#
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; } // Required foreign key property
    public Blog Blog { get; set; } = null!; // Required reference navigation to principal
}
```
Có thể dùng API Fluent để cấu hình 
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Post>()
        .HasOne(e => e.Blog)
        .WithMany()
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```
Lưu ý hàm WithMany() không có tham số để EF hiểu là không có sự điều
hướng từ entity chính sang entity phụ

Nếu đứng từ bảng chính cấu hình thì phải được chỉ định rõ ràng bằng cách sử dụng lệnh HasMany<>()gọi chung
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany<Post>()
        .WithOne(e => e.Blog)
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```

## V. Quan hệ một-nhiều không có điều hướng
Đôi khi, việc cấu hình một mối quan hệ không có điều hướng có thể hữu ích. Mối quan hệ như vậy chỉ có thể được thao tác bằng cách thay đổi trực tiếp giá trị khóa ngoại.

``` C#
// Principal (parent)
public class Blog
{
    public int Id { get; set; }
}

// Dependent (child)
public class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; } // Required foreign key property
}
```

Có thể sử dụng API Fluent cấu hình như sau
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany<Post>()
        .WithOne();
}
```

Với cấu hình này, thuộc tính Post.BlogId vẫn được nhận diện là khóa ngoại theo quy ước, và mối quan hệ là bắt buộc vì thuộc tính khóa ngoại không cho phép giá trị null
<br/>
Nếu muốn sử dụng với thuộc tính cho khóa ngoại được phép mang giá trị null thì
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Blog>()
        .HasMany<Post>()
        .WithOne()
        .HasForeignKey(e => e.BlogId)
        .IsRequired();
}
```