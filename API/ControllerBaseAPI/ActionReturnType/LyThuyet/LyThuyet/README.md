# Controller action return types in ASP.NET Core web API

ASP.NET Core cung cấp các tùy chọn sau cho kiểu trả về của hành động controller API web:
- Specific type
- IActionResult
- ActionResult < T >
- HttpResults


## I. Specific type 
Thao tác cơ bản nhất trả về một kiểu dữ liệu nguyên thủy hoặc phức tạp, 

ví dụ 

        [HttpGet]
        public Task<List<Product>> Get() {}

Khi nào nên dùng? Khi hàm của bạn chắc chắn 100% chỉ có 1 kịch bản trả về 
thành công. Vì hàm không nhận tham số đầu vào (không sợ lỗi nhập liệu) và cũng không có logic rẽ nhánh, nên không cần lo trả về mã lỗi 400 Bad Request hay 404 Not Found.

## II. ActionResult vs IActionResult

