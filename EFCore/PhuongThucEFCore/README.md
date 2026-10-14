# Kiến thức về EF Core
## 1. Nhóm phương thức Thêm / Sửa / Xóa (CRUD Write Operations)
Các phương thức này dùng để thay đổi dữ liệu trong bộ nhớ. Dữ liệu chỉ thực sự được ghi xuống cơ sở dữ liệu khi gọi SaveChangesAsync()
- Add() / AddAsync() & AddRange() / AddRangeAsync()
	+ Chức năng: Đêm một hoặc nhiều đối tượng mới vào DbSet, đánh dấu trạng thái entity là Added
``` C#
// Thêm 1 bản ghi
var newMovie = new Movie { Title = "Inception", Price = 9.99m };
_context.Movie.Add(newMovie); // Hoặc await _context.Movie.AddAsync(newMovie); [1]

// Thêm danh sách nhiều bản ghi
var movies = new List<Movie>
{
    new Movie { Title = "Avatar", Price = 12.99m },
    new Movie { Title = "Titanic", Price = 8.99m }
};
_context.Movie.AddRange(movies); // [3, 4]
```
- Update() / UpdateRange()
    + Chức năng: Đánh dấu đối tượng đã bị thay đổi (trạng thái Modified) để EF Core sinh ra lệnh SQL UPDATE
``` C#
var movie = await _context.Movie.FindAsync(id); // [7]
movie.Price = 10.99m;
_context.Movie.Update(movie);
```
- Remove() / RemoveRange()
   + Chức năng: Đánh dấu đối tượng cần xóa (trạng thái Deleted) để EF Core sinh ra lệnh SQL DELETE
``` C#
var student = await _context.Students.FindAsync(id); // [10]
if (student != null)
{
    _context.Students.Remove(student); // [8, 9]
}
```
- Attach()
   + Chức năng: Gắn một đối tượng chưa được theo dõi (Detached) vào DbContext để quản lý trạng thái thủ công
``` C#
// Nhận đối tượng từ Form/API gửi lên
_context.Attach(Movie).State = EntityState.Modified;
```
- SaveChanges() / SaveChangesAsync()
   + Chức năng: Mở kết nối, tạo Transaction, biên dịch trạng thái Change Tracker thành các lệnh SQL (INSERT, UPDATE, DELETE) và thực thi xuống Database
 ``` C#
 await _context.SaveChangesAsync();
 ```
 ## 2.  Nhóm phương thức Truy vấn & Đọc dữ liệu (LINQ & Execution)
 Các phương thức xây dựng câu truy vấn (Where, Select, OrderBy) sẽ tạo thành cây biểu thức IQueryable (Deferred Execution) và chỉ thực sự truy vấn Database khi gọi các phương thức thực thi (ToListAsync, FirstOrDefaultAsync,...)
 - ToListAsync() / ToArrayAsync()
   + Chức năng: Thực thi câu lệnh SQL SELECT và ép kết quả về danh sách C#
``` C#
    List<Movie> movies = await _context.Movie.Where(m => m.Price > 5).ToListAsync();
```
- FirstOrDefaultAsync() / SingleOrDefaultAsync()
    + Chức năng:
        + FirstOrDefaultAsync: Lấy bản ghi đầu tiên thỏa điều kiện, nếu không có trả về null
        + SingleOrDefaultAsync: Lấy bản ghi duy nhất. Báo lỗi exception nếu có nhiều hơn 1 bản ghi thỏa điều kiện
``` C#
var student = await _context.Students.FirstOrDefaultAsync(s => s.ID == id);
```
- FindAsync()
   + Chức năng: Tìm bản ghi theo Khóa chính (Primary Key). Phương thức này tối ưu hơn ở chỗ nó sẽ kiểm tra trong bộ nhớ cache của DbContext trước, nếu chưa có mới gửi truy vấn SQL đến DB
``` C#
var movie = await _context.Movie.FindAsync(id);
```
- AnyAsync() / CountAsync()
   + Chức năng:
       + AnyAsync: Trả về true/false kiểm tra xem có bản ghi nào thỏa điều kiện hay không
       + CountAsync: Trả về tổng số lượng bản ghi
```C#
bool exists = await _context.Movie.AnyAsync(e => e.Id == id);
int totalMovies = await _context.Movie.CountAsync();
```
- Skip() & Take()
   + Chức năng: Dùng để phân trang (Paging)
```C#
int pageSize = 10;
int pageIndex = 1;

var pageData = await _context.Students
    .Skip((pageIndex - 1) * pageSize) // Bỏ qua các trang trước
    .Take(pageSize)                   // Lấy số bản ghi của trang hiện tại
    .ToListAsync();
```
## 3. Nhóm phương thức Tối ưu hiệu năng & Quản lý Trạng thái
- AsNoTracking()
    + Chức năng: Tắt tính năng theo dõi thay đổi (Change Tracker) của DbContext. Áp dụng cho các truy vấn chỉ đọc (Read-only) để tăng tốc độ thực thi và tiết kiệm bộ nhớ RAM
``` C#
var students = await _context.Students
    .AsNoTracking() // Tối ưu performance khi chỉ hiển thị danh sách [24-26]
    .ToListAsync();
```
- Entry()
    + Chức năng: Truy cập vào metadata và bộ theo dõi trạng thái ChangeTracker của một entity cụ thể
``` C#
// Thay đổi trạng thái thủ công
_context.Entry(movie).State = EntityState.Modified; // [11, 27]

// Kiểm tra xem một thuộc tính có bị sửa đổi hay không
bool isTitleModified = _context.Entry(movie).Property(m => m.Title).IsModified;
```
## 4. Nhóm phương thức Nạp dữ liệu liên quan (Loading Related Data)
- Include() & ThenInclude() (Eager Loading)
   + Chức năng: Tải dữ liệu từ các bảng quan hệ ngay trong một câu lệnh JOIN SQL duy nhất
       + Include: Tải bảng con cấp 1
       + ThenInclude: Tải tiếp bảng con cấp 2 nối từ bảng cấp 1
```C#
// Lấy Student kèm danh sách Enrollments, và trong từng Enrollment lấy luôn thông tin Course [24]
var studentDetails = await _context.Students
    .Include(s => s.Enrollments)
        .ThenInclude(e => e.Course) 
    .FirstOrDefaultAsync(s => s.ID == id);
```
- Explicit Loading (LoadAsync)
  + Chức năng: Chủ động phát lệnh tải dữ liệu bảng con cho một đối tượng đã có sẵn trong bộ nhớ
```C#
  var student = await _context.Students.FindAsync(id); // [10]

// Chủ động tải danh sách Enrollments của student này
await _context.Entry(student)
    .Collection(s => s.Enrollments)
    .LoadAsync();
```
## 5. Nhóm phương thức SQL thuần & Quản lý Database
- FromSqlRaw() / FromSqlInterpolated()
    + Chức năng: Viết trực tiếp câu lệnh SQL SELECT tùy chỉnh để ánh xạ về Entity
``` C#
var searchTitle = "Ghostbusters";
var movies = await _context.Movie
    .FromSqlInterpolated($"SELECT * FROM Movie WHERE Title = {searchTitle}")
    .ToListAsync();
```
- ExecuteSqlRawAsync() / ExecuteSqlInterpolatedAsync()
    + Chức năng: Thực thi các lệnh SQL không trả về Entity (như UPDATE, DELETE hàng loạt) trực tiếp xuống DB mà không thông qua Change Tracker
``` C#
int rowsAffected = await _context.Database
    .ExecuteSqlInterpolatedAsync($"UPDATE Movie SET Price = Price * 1.1 WHERE Genre = 'Comedy'");
```
- Database.EnsureCreated() & Database.MigrateAsync()
    + Chức năng:
       + EnsureCreated(): Tự động tạo DB nếu chưa tồn tại (thường dùng cho In-Memory / Testing)
       + MigrateAsync(): Áp dụng các file Migration chưa chạy xuống DB thực tế
``` C#
await _context.Database.MigrateAsync();
```