# Cấu hình tạo mối quan hệ Many to Many
## Cấu hình qua thuộc tính không có bảng trung gian
- Không cần khai báo lớp Entity trung gian trong C# mà 
EF Core sẽ tự cấu hình tạo bảng database trung gian
- Cách 1 sửa dụng thuộc tính trong lớp Enitty
``` C#
public class Course
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
    
    // Navigation property trỏ trực tiếp đến Instructor
    public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}

public class Instructor
{
    public int ID { get; set; }
    public string LastName { get; set; } = string.Empty;
    
    // Navigation property trỏ trực tiếp đến Course
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
```
- Cách 2 sử dụng qua cấu hình Fluent API

```
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Course>()
        .HasMany(c => c.Instructors)
        .WithMany(i => i.Courses)
        .UsingEntity(j => j.ToTable("CourseInstructor")); // Tùy chỉnh tên bảng trung gian
}
```

## Cấu hình có bảng trung gian chứa thông tin bổ sung
Dạng cấu hình này chứa thông tin bổ sung cần thiết nếu muốn
dùng cấu hình thêm thuộc tính cho bảng trung gian
``` C#
// 1. Entity chính 1
public class Student
{
    public int ID { get; set; }
    public string LastName { get; set; } = string.Empty;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

// 2. Entity chính 2
public class Course
{
    public int CourseID { get; set; }
    public string Title { get; set; } = string.Empty;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}

// 3. Entity trung gian (chứa thuộc tính bổ sung Grade)
public class Enrollment
{
    public int EnrollmentID { get; set; }
    public int CourseID { get; set; }
    public int StudentID { get; set; }
    
    public Grade? Grade { get; set; } // <--- Payload (Thông tin bổ sung)

    public Course Course { get; set; } = null!;
    public Student Student { get; set; } = null!;
}
```
Ở dạng cấu hình này cần cấu hình tạo khóa tổng hợp trong bảng
trung gian thông qua cấu hình Fluent API
``` C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Tạo khóa chính tổ hợp từ 2 cột Khóa ngoại
    modelBuilder.Entity<Enrollment>()
        .HasKey(e => new { e.StudentID, e.CourseID });

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Student)
        .WithMany(s => s.Enrollments)
        .HasForeignKey(e => e.StudentID);

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Course)
        .WithMany(c => c.Enrollments)
        .HasForeignKey(e => e.CourseID);
}
```
## Thao tác insert dữ liệu
- Dạng không có bảng trung gian 
``` C#
var course = await _context.Courses.FindAsync(courseId);
var instructor = await _context.Instructors.FindAsync(instructorId);

course.Instructors.Add(instructor);
await _context.SaveChangesAsync();
```
- Dạng có bảng trung gian thì phải tạo đối tượng trung gian
``` C#
var enrollment = new Enrollment
{
    StudentID = studentId,
    CourseID = courseId,
    Grade = Grade.A
};
_context.Enrollments.Add(enrollment);
await _context.SaveChangesAsync();
```


