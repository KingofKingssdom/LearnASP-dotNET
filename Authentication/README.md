# Tổng quan về xác thực (authentication) trong ASP .NET
- Xác thực là quá trình xác định danh tính của người dùng. Trong ASP.NET Core, việc xác thực được xử lý bởi 
service xác thực. Được sử dụng bởi các middleware xác thực
- Các trình xử lý xác thực (JWT hoặc Cookie) đã đăng ký và các tùy chọn cấu hình của chúng được gọi là "lược đồ".
- Các lược đồ xác thực được chỉ định bằng cách đăng ký các dịch vụ xác thực trong Program.cs:
	+ Bằng cách gọi một phương thức mở rộng dành riêng cho từng lược đồ sau khi gọi AddAuthentication , chẳng hạn như AddJwtBearer hoặc AddCookie . Các phương thức mở rộng này sử dụng AuthenticationBuilder.AddScheme để đăng ký các lược đồ với các thiết lập phù hợp.
	+ Ít phổ biến hơn là bằng cách gọi điện AuthenticationBuilder.AddScheme trực tiếp.

Xác thực là gì, khi user đăng kí vào thông tin sẽ được lưu xuống database. Khi user đăng nhập
là lúc xác thực xuất hiện user nhập thông tin nếu khớp với thông tin trong db nó sẽ bắn ra một
token (nếu dùng jwt) hoặc cookie để user giữ trong trình duyệt. Khi người dùng gửi các request khác
cái token hoặc cookie sẽ đc đính kèm trong header để gửi đi kèm. Từ đó biết danh tính user này là
ai đã đăng nhập chưa.
## I. Khái niệm
Quá trình xác thực chịu trách nhiệm cung cấp ClaimsPrincipal để ủy quyền và đưa ra quyết định về quyền hạn. Có nhiều phương pháp lược đồ xác thực khác nhau để lựa chọn trình xử lý xác thực nào chịu trách nhiệm tạo ra tập hợp các yêu cầu (claims) chính xác

**a. Claims là gì** <br/>
Claim là một thông tin (một "lời khẳng định") về người dùng. Có thể hiểu
làm một mẩu dữ liệu mô tả về người dùng <br/>
Ví dụ sau khi người dùng đăng nhập thành công, hệ thống biết: <br/>
Id = 15 <br/>
Username = huy <br/>
Email = huy@gmail.com <br/>
Role = Admin <br/>
Quốc gia = Việt Nam <br/>

Mỗi thông tin trên chính là một Claim. Mỗi key nó gọi là ClaimType, mỗi giá trị là một value
ClaimIdentity là một tập hợp gồm nhiều Claim. Một ClaimsPrincipal đại diện cho người dùng hiện tại.
Bên trong nó chứa một hoặc nhiều ClaimsIdentity.

