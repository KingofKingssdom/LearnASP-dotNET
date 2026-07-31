# Các loại Entity trong EF Core
## I. Bao gồm các loại trong model
Entity được tạo và được EF Core phát hiện ra nếu khai báo 
+ nằm trong DBSet
``` C#
public DbSet<Blog> Blogs { get; set; }
```
+ Class thực thể được tạo nối với class khác. Thì class khác đó nó cũng sẽ được tạo luôn thành Entity
``` C#
public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public Blog Blog { get; set; }
}
// Entity Post cũng được phát hiện và tạo vì nó liên quan đến Blog
```

+ Khai báo một Entity nằm trong OnModelCreating
``` C# 
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditEntry>();
    }
```

Nếu trong Class này phải bao gồm class khác nhưng không muốn tạo Entity dính đó thì 
có thể sử dụng để loại bỏ: 
+ Cách 1 sử dụng Annotation
```C#
[NotMapped]
public class BlogMetadata
{
    public DateTime LoadedFromDatabase { get; set; }
}
```
+ Cách 2 sử dụng API Fluent
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Ignore<BlogMetadata>();
}
```

## II. Đặt tên Entity
+ Theo mặc định mỗi bảng trong database sẽ đc đặt tên theo Dbset`<Entity>`
nhưng nếu không đặt DBSet thì tên thực thể sẽ được đặt theo tên của class
+ Ngoài ra có thể tự ý đổi tên bảng như sau:
    + Sử dụng annotation

    ``` C#
    [Table("blogs")]
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
    }
    ```
    + Sử dụng API Fluent
    ``` C#
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>()
            .ToTable("blogs");
    }
    ```
