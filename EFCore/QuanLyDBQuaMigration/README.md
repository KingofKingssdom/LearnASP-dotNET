# Quản lý Database thông qua Migration
## I. Khái niệm
- Migration trong Entity Framework Core (EF Core) là tính năng giúp quản lý và tự động hóa việc cập nhật cấu trúc cơ sở dữ liệu (Database Schema) mỗi khi mô hình dữ liệu (Data Model / Lớp C#) trong ứng dụng của bạn thay đổi
- Thay vì phải xóa rồi tạo lại cơ sở dữ liệu (gây mất dữ liệu) hoặc viết các câu lệnh SQL DDL (CREATE TABLE, ALTER TABLE) thủ công, Migration giúp đồng bộ hóa mô hình C# và Database một cách tăng tiến mà vẫn bảo toàn dữ liệu hiện có
## II. Cơ chế hoạt động của Migration trong hệ thống

Để theo dõi và áp dụng các thay đổi theo thời gian, EF Core sử dụng 3 thành phần cốt lõi:
- Thành phần Snapshot (ModelSnapshot.cs):
	+ EF Core tự động tạo và duy trì một file Snapshot đại diện cho toàn bộ cấu trúc Data Model ở trạng thái mới nhất
	+ Mỗi khi bạn tạo một Migration mới, EF Core sẽ so sánh lớp DbContext hiện tại với file Snapshot này để phát hiện chính xác những gì đã thay đổi (thêm bảng, sửa cột, thêm khóa ngoại,...)
- File mã nguồn Migration (Timestamp_MigrationName.cs):
	+ Dựa trên sự khác biệt phát hiện được, EF Core sinh ra một file code C# chứa 2 phương thức chính
		+ Up(): Chứa các lệnh cập nhật cấu trúc DB lên phiên bản mới (ví dụ: migrationBuilder.CreateTable, migrationBuilder.AddColumn)
		+ Down(): Chứa các lệnh hoàn tác (rollback) để đưa cấu trúc DB quay trở lại trạng thái trước khi áp dụng Migration này (ví dụ: migrationBuilder.DropTable, migrationBuilder.DropColumn)
- Bảng lịch sử trong Database (__EFMigrationsHistory):
	+ Khi bạn khởi tạo Database bằng Migration, EF Core tự động tạo ra một bảng hệ thống tên là __EFMigrationsHistory trong CSDL
	+ Bảng này chỉ gồm 2 cột: MigrationId (mã định danh kèm thời gian) và ProductVersion
	+ Mỗi khi chạy lệnh cập nhật CSDL, EF Core sẽ tra cứu bảng này để biết những Migration nào đã chạy rồi và chỉ thực thi các Migration mới chưa được áp dụng
## III. Luồng hoạt động của Migration
+ Bước 1: Cập nhật C# Entity Model
```C#
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Rating { get; set; } // Thuộc tính mới thêm
}
```
+ Bước 2: Tạo file Migration mới EF Core so sánh Model C# mới với Snapshot và tạo ra bản ghi thay đổi
  + Package Manager Console (PMC): Add-Migration AddRatingToMovie
  + .NET Core CLI: dotnet ef migrations add AddRatingToMovie
+ Bước 3: Kiểm tra code Migration được sinh ra Bạn kiểm tra file trong thư mục Migrations/ để đảm bảo lệnh SQL/C# sinh ra đúng ý đồ và không có cảnh báo mất dữ liệu
+ Bước 4: Cập nhật thay đổi xuống CSDL EF Core đọc các lệnh trong phương thức Up() và thực thi xuống CSDL
	+ Package Manager Console (PMC): Update-Database
	+ .NET Core CLI: dotnet ef database update

## IV. Các câu lệnh Migration thường dùng
|Thao tác| Package Manager Console (VS)| .NET Core CLI|
|---|---|---|
|Tạo migration mới| Add-Migration NameTuDat| dotnet ef migrations add NameTuDat|
|Cập nhật CSDL| Update-Database|dotnet ef database update|
|Xóa migration vừa tạo (chưa apply)|Remove-Migration|dotnet ef migrations remove|
|Xuất file script SQL (cho Prod)|Script-Migration|dotnet ef migrations script|
|Xóa sạch CSDL|Drop-Database|dotnet ef database drop|

