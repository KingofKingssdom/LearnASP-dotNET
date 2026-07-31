# EF Core NuGet Packages
EF core được phân phối trong package NuGet. Các package cần thiết cho 
một ứng dụng phụ thuộc vào
+ Loại hệ thống cơ sở dữ liệu đang được sử dụng (SQL Server, SQLite, v.v.)
+ Các tính năng EF Core cần thiết

## I. Phiên bản
Đảm bảo phải cài đặt đúng các version dành cho toàn bộ gói trong một ứng dụng
## II. Các gói cài đặt cho cơ sở dữ liệu
|Hệ thống cơ sở dữ liệu|Package|
|---|---|
|SQL Server và SQL Azure| Microsoft.EntityFrameworkCore.SqlServer|
|SQLite|Microsoft.EntityFrameworkCore.Sqlite|
|PostgreSQL|Npgsql.EntityFrameworkCore.PostgreSQL|
|MySQL|Pomelo.EntityFrameworkCore.MySql|
|Azure Cosmos DB|Microsoft.EntityFrameworkCore.Cosmos|
|Cơ sở dữ liệu trong bộ nhớ EF Core**|Microsoft.EntityFrameworkCore.InMemory|

## III. Các gói Tools
+ **Microsoft.EntityFrameworkCore.Tools** :  dùng để chạy các lệnh quản lý cơ sở dữ liệu trong Entity Framework Core như Migrations, Scaffolding và cập nhật cấu trúc bảng từ mã nguồn C#
