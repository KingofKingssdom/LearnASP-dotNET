# Cách cài đặt EF Core cho dự án
## I. Khái niệm về Entity Framework 
Entity Framework (EF) là một công cụ ORM (Object-Relational Mapper) giúp lập trình viên viết mã C# để quản lý cơ sở dữ liệu thay vì dùng câu lệnh SQL. Các đặc điểm chính gồm: ánh xạ dữ liệu, hỗ trợ LINQ, và quản lý kết nối tự động <br/>

**Đặc điểm nổi bật**
+ Ánh xạ đối tượng: Biến các bảng trong cơ sở dữ liệu thành các lớp (class) và các hàng thành các đối tượng (object) trong code.
+ Truy vấn LINQ: Cho phép tìm kiếm, thêm, sửa, xóa dữ liệu bằng ngôn ngữ C# thuần thay vì viết câu lệnh SQL dài.
+ Tự động hóa: Hỗ trợ tạo cơ sở dữ liệu từ code (Code-First) hoặc tạo code từ cơ sở dữ liệu có sẵn (Database-First)

**Các cách tiếp cận chính**
+ Code-First: Viết các lớp C# trước rồi dùng EF tạo ra các bảng dữ liệu tương ứng.
+ Database-First: Dùng cơ sở dữ liệu có sẵn để EF tự động sinh ra các lớp C# cho ứng dụng

## II. Cách cài đặt EF cho dự án
EF Core được phân phối dưới dạng packet NuGet. Để thêm EF Core vào ứng dụng, hãy cài đặt gói NuGet cho loại cơ sở dữ liệu mà muốn sử dụng

Có 2 cách tạo:
+ Cách 1: Sử dụng dòng lệnh CLI trong terminal
	+ Sử dụng dòng lệnh: 
	``` PowerShell 
	dotnet add package Microsoft.EntityFrameworkCore.SqlServer 
	```
+ Cách 2: Sử dụng trình quản lý packet NuGet trong Visual studio
	+ Từ menu Visual Studio, chọn Project > Manage NuGet Packages
	+ Nhấp vào tab browse
	+ Để cài đặt hoặc cập nhật trình cung cấp SQL Server, hãy chọn
		``` C# 
		Microsoft.EntityFrameworkCore.SqlServer
		```
	+ Chọn phiên bản và bấm install

Cài đặt thêm 
Gói Microsoft.EntityFrameworkCore.Tools dùng để chạy các lệnh quản lý cơ sở dữ liệu trong Entity Framework Core như Migrations, Scaffolding và cập nhật cấu trúc bảng từ mã nguồn C#