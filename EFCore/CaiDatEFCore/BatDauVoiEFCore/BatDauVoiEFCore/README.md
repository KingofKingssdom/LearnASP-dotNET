# Ghi chú 
- Sử dụng database SQLite để chạy
- Trong dòng terminal Package Manager Console dùng để chạy các dòng lệnh để chạy cài đặt EF Core
``` C#
Add-Migration InitialCreate
Update-Database
```
Lệnh Add-Migration Nó tạo một migration để thiết lập tập hợp các bảng ban đầu cho mode
Lệnh Update-Database này tạo cơ sở dữ liệu và áp dụng migration mới vừa tạo trên vào đó
