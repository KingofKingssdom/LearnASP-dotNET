# Hiểu về Controller và Routing trong ASP .Net core
## I. Thẻ [ApiController] và [Route]
Khi tạo một Web API Controller, lớp (class) của bạn bắt buộc kế thừa từ ControllerBase và đính kèm 2 attribute quan trọng này ở cấp Class:
```C#
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController] // 1. Bật các tính năng tự động của Web API
    [Route("api/[controller]")] // 2. Định nghĩa đường dẫn gốc (Base Route)
    public class ProductsController : ControllerBase
    {
        // ... các action
    }
}
```
### A. Tác dụng của [ApiController]
- Tự động Validate dữ liệu (ModelState): Ngay khi dữ liệu gửi lên vi phạm quy tắc (ví dụ: thiếu trường [Required]), API sẽ tự động trả về lỗi 400 Bad Request kèm chi tiết mà bạn không cần phải viết thủ công if (!ModelState.IsValid).
- Tự động đoán nguồn Binding (Parameter Binding Inference): Tự động hiểu các kiểu phức tạp (Complex Types/Objects) lấy từ Request Body, còn các kiểu đơn giản (int, string, Guid) lấy từ Route/Query.
- Tự động xử lý Form Data ([FromForm]): Tự động nhận diện khi có upload file (IFormFile).
- Chuẩn hóa phản hồi lỗi: Trả về cấu trúc lỗi theo chuẩn Problem Details (RFC 7807).

### B. Tác dụng của [Route]
- Quy định mẫu đường dẫn URL để truy cập vào Controller.
- Mẫu [Route("api/[controller]")]: Token [controller] sẽ tự động lấy tên của Class bỏ đi chữ "Controller"</br>
  Ví dụ: ProductsController -> đường dẫn gốc sẽ là /api/products

## II Thuộc tính truyền tham số (Parameter Binding Attributes)
ASP.NET Core cung cấp các attribute để chỉ định chính xác vị trí mà Action sẽ rút trích dữ liệu từ HTTP Request gửi lên:
### 1. [FromRoute] — Lấy dữ liệu từ trên đường dẫn URL (Path Segment)
- Dùng cho các thông tin định danh nằm trên URL (như ID).
  + URL ví dụ: /api/products/15
  + Khai báo: 
  ```
  [HttpGet("{id}")] 
  public IActionResult GetById([FromRoute] int id)
  ```
### 2. [FromQuery] — Lấy dữ liệu từ chuỗi truy vấn (Query String)
- Dùng cho các tham số lọc, tìm kiếm, phân trang nằm sau dấu ?
  + URL ví dụ: /api/products?page=1&pageSize=10&search=laptop
  + Khai báo: 
  ``` 
  [HttpGet] 
  public IActionResult GetList([FromQuery] int page, [FromQuery] string search)
  ```
### 3. [FromBody] — Lấy dữ liệu từ Request Body
- Dùng cho dữ liệu dạng JSON/XML gửi ẩn bên trong Body của Request (thường dùng cho POST, PUT, PATCH).
  + Request Body: {"name": "iPhone 15", "price": 999}
  + Khai báo:
  ```
  [HttpPost] 
  public IActionResult Create([FromBody] CreateProductDto dto)
  ```
### 4. [FromHeader] — Lấy dữ liệu từ HTTP Header
- Dùng để đọc các thông tin cấu hình, Token, User-Agent, hoặc Custom Header từ Header của Request.
  + Header: X-Device-Id: 12345
  + Khai báo
  ```
  public IActionResult Check([FromHeader(Name = "X-Device-Id")] string deviceId)
  ```
### 5. [FromForm] - Nhận dữ liệu gửi lên là dạng Form
- Dùng để đọc body gửi lên là form data 
  + Request Body: là dạng form-data
  + Khai báo
  ```
  [HttpPost] 
  public IActionResult Create([FromForm] CreateProductDto dto)
  ```
## III. Action Result
  Trong ASP.NET Core Web API, Action Result đại diện cho phản hồi HTTP (HTTP Response) mà Controller trả về cho Client. Nó không chỉ chứa dữ liệu (JSON, XML, file...) mà còn kèm theo mã trạng thái HTTP (HTTP Status Code) để thông báo kết quả xử lý của server.
### 1. So sánh 3 kiểu trả về phổ biến trong Controller

  |Kiểu trả về|Ưu điểm|Nhược điểm|Trường hợp sử dụng|
  |---|---|---|---|
  |Kiểu cụ thể (T / ```IEnumerable<T>```)|Đơn giản, tự động map kiểu dữ liệu cho Swagger/OpenAPI.|Không thể trả về các HTTP Status Code khác nhau (như 404 Not Found hay 400 Bad Request)|Chỉ dùng cho các action cực kỳ đơn giản luôn trả về 200 OK|
  |IActionResult|Rất linh hoạt, cho phép trả về nhiều loại Status Code khác nhau (Ok, NotFound, BadRequest)|Swagger không tự hiểu được kiểu dữ liệu DTO trả về là gì (phải gắn thêm [ProducesResponseType])|Dùng khi action có thể trả về nhiều kiểu dữ liệu/status code khác nhau.|
  |```ActionResult<T>``` (Khuyên dùng cho Web API)|Kết hợp ưu điểm của cả hai: Vừa linh hoạt trả về các Helper Method (NotFound(), BadRequest()), vừa khai báo rõ kiểu dữ liệu T cho Swagger.|Cần khai báo đúng kiểu T ở chữ ký hàm.|Chuẩn mực cho Web API, vừa linh hoạt vừa hỗ trợ tài liệu OpenAPI/Swagger tốt nhất.|
### 2. tổng hợp các phương thức Helper & Mã HTTP Status Code chuẩn
  ASP.NET Core cung cấp sẵn các phương thức Helper trong ControllerBase để tạo nhanh các Action Result tương ứng với mã HTTP:

A. Nhóm 2xx — Thành công (Success)
   - 200 OK — Ok() / Ok(object): Trả về dữ liệu kèm mã thành công 200.
   - 201 Created — CreatedAtAction(...) / CreatedAtRoute(...): Trả về khi tạo mới thành công một tài nguyên. Tự động đính kèm đường dẫn URI đến tài nguyên mới tạo trong Header Location
   - 204 No Content — NoContent(): Yêu cầu đã xử lý thành công nhưng không cần trả về dữ liệu trong Body (thường dùng sau khi PUT cập nhật hoặc DELETE xóa).
B. Nhóm 4xx — Lỗi từ Client (Client Errors)
   - 400 Bad Request — BadRequest() / BadRequest(modelState): Dữ liệu client gửi lên không hợp lệ hoặc vi phạm logic kiểm tra.
   - 401 Unauthorized — Unauthorized(): Người dùng chưa đăng nhập / thiếu Token xác thực
   - 403 Forbidden — Forbid(): Người dùng đã đăng nhập nhưng không đủ quyền (Role/Policy) truy cập tài nguyên.
   - 404 Not Found — NotFound() / NotFound(message): Không tìm thấy tài nguyên yêu cầu trong Database.
   - 409 Conflict — Conflict(message): Dữ liệu gửi lên bị xung đột (ví dụ: đăng ký Email đã tồn tại).
C. Nhóm 5xx — Lỗi từ Server (Server Errors)
   - 500 Internal Server Error — Problem(...) / StatusCode(500, message): Lỗi hệ thống không lường trước (Exception) xảy ra trong quá trình xử lý trên server.
### 3. Chi tiết sự khác biệt: IActionResult vs ActionResult<T>
Ví dụ dùng IActionResult:
```C#
[HttpGet("{id}")]
public IActionResult GetProduct(int id)
{
    var product = _repository.GetById(id);
    if (product == null)
    {
        return NotFound($"Không tìm thấy sản phẩm ID = {id}"); // Trả về 404
    }
    return Ok(product); // Trả về 200 kèm object
}
```
Điểm yếu: Swagger chỉ biết hàm này trả về một HTTP Response chung chung, không biết cụ thể đối tượng Product có những trường nào ngoại trừ khi bạn đính kèm thuộc tính [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
</br>
Ví dụ dùng ActionResult<T> (Tối ưu nhất):
```C#
[HttpGet("{id}")]
public ActionResult<ProductDto> GetProduct(int id)
{
    var product = _repository.GetById(id);
    if (product == null)
    {
        return NotFound(); // Trả về 404 (Implicit conversion sang ActionResult<ProductDto>)
    }
    
    // Có thể trả trực tiếp đối tượng product mà không cần bọc Ok(product)
    return product; // Trả về 200 OK + ProductDto (Tự động chuyển đổi)
}
```
### 4. Controller mẫu thực tế tổng hợp đầy đủ các Action Results
```
using Microsoft.AspNetCore.Mvc;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        // 1. Trả về 200 OK với danh sách
        [HttpGet]
        public ActionResult<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = new List<OrderDto> { new(1, "Order #1", 150.0m) };
            return Ok(orders); // 200 OK
        }

        // 2. Trả về 200 OK hoặc 404 Not Found
        [HttpGet("{id:int}")]
        public ActionResult<OrderDto> GetOrderById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID đơn hàng phải lớn hơn 0"); // 400 Bad Request
            }

            var order = id == 1 ? new OrderDto(1, "Order #1", 150.0m) : null;
            if (order == null)
            {
                return NotFound(new { Message = $"Đơn hàng {id} không tồn tại" }); // 404 Not Found
            }

            return order; // 200 OK (Trả thẳng DTO)
        }

        // 3. Trả về 201 Created khi thêm mới thành công
        [HttpPost]
        public ActionResult<OrderDto> CreateOrder([FromBody] CreateOrderDto dto)
        {
            // Giả lập lưu vào DB
            var newOrder = new OrderDto(10, dto.OrderName, dto.Amount);

            // Trả về 201 Created kèm Header Location trỏ đến API GetOrderById(10)
            return CreatedAtAction(
                nameof(GetOrderById), 
                new { id = newOrder.Id }, 
                newOrder); 
        }

        // 4. Trả về 204 No Content khi cập nhật thành công
        [HttpPut("{id:int}")]
        public IActionResult UpdateOrder(int id, [FromBody] UpdateOrderDto dto)
        {
            var exists = id == 1;
            if (!exists) return NotFound(); // 404

            // Thực hiện cập nhật...
            return NoContent(); // 204 No Content
        }

        // 5. Trả về 204 No Content hoặc 500 khi có lỗi
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                // Thực hiện xóa...
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                // Trả về 500 Internal Server Error
                return Problem(
                    detail: ex.Message, 
                    statusCode: StatusCodes.Status500InternalServerError, 
                    title: "Lỗi hệ thống khi xóa đơn hàng");
            }
        }
    }

    public record OrderDto(int Id, string OrderName, decimal Amount);
    public record CreateOrderDto(string OrderName, decimal Amount);
    public record UpdateOrderDto(string OrderName, decimal Amount);
}
```