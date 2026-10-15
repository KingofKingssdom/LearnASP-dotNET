# JWT (Json Web Token)
## I. Khái niệm
Là một phương pháp nhỏ gọn và an toàn để truyền tải thông tin giữa các bên dưới dạng đối tượng JSON. Nó thường được sử dụng để xác thực và ủy quyền trong các ứng dụng web, cho phép người dùng truy cập các tài nguyên được bảo vệ mà không cần phải cung cấp thông tin đăng nhập nhiều lần. Các tính năng chính bao gồm:
+ Xác thực không trạng thái: Lưu trữ thông tin người dùng trong mã thông báo, giảm nhu cầu lưu trữ phiên trên máy chủ. (nghĩa là đối với stateful thì server phải lưu thông tin
của client trong session. Còn với JWT thì stateless nghĩa là không cần lưu gì, chỉ cần giải mã signature)
+ Trao đổi dữ liệu an toàn: Sử dụng chữ ký số để xác minh rằng mã thông báo không bị thay đổi.
+ Định dạng nhỏ gọn: Bao gồm ba phần—Tiêu đề, Nội dung và Chữ ký—được mã hóa thành một chuỗi duy nhất.
+ Được hỗ trợ rộng rãi: Hoạt động trên nhiều ngôn ngữ lập trình và nền tảng khác nhau.

## II. Cấu trúc
JWT cấu trúc gồm 3 thành phần HEADER.PAYLOAD.SIGNATURE

**Header:** <br/>
+ alg: Thuật toán được sử dụng để ký (ví dụ: HS256, RS256).
+ typ: Loại token, luôn là "JWT".

Sẽ tạo ra một mã ví dụ eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9

**Payload:** chứa thông tin về người dùng, còn được gọi là yêu cầu xác thực, và một số thông tin bổ sung bao gồm dấu thời gian phát hành và thời gian hết hạn của mã thông báo. <br/>

Các thông tin bổ sung

+ iss (Issuer): Xác định đơn vị phát hành token.
+ sub (Chủ thể): Đại diện cho người dùng hoặc thực thể mà mã thông báo này đề cập đến.
+ aud (Đối tượng): Chỉ định người nhận dự định.
+ exp (Thời hạn): Xác định thời điểm mã thông báo hết hạn.
+ iat (Issued At): Thời điểm mã thông báo được tạo.
+ nbf (Not Before) : Chỉ định thời điểm mã thông báo trở nên hợp lệ.

Sẽ tạo ta một mã ví dụ eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNzA4MzQ1MTIzLCJleHAiOjE3MDgzNTUxMjN9

**Signature**: Kết tợp header, payload và serectkey để tạo ra một mã kí tự

ví dụ: SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

Cuối cùng 3 mã trên kết hợp lại thành một token gửi cho client

eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNzA4MzQ1MTIzLCJleHAiOjE3MDgzNTUxMjN9.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

## III. Cách cấp phát jwt trong ASP .Net
Khi người dùng gửi thành công Username/Password ở API /api/auth/login, bạn tạo và trả về chuỗi JWT Token như sau:
```C#
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public string GenerateJwtToken(string userId, string email, string role, IConfiguration config)
{
    // 1. Lấy Secret Key từ appsettings.json (độ dài tối thiểu 16-32 ký tự)
    var secretKey = config["JwtSettings:SecretKey"]!;
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    // 2. Định nghĩa các thông tin (Claims) chứa trong Token
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(ClaimTypes.Role, role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    // 3. Cấu hình thông số Token
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddHours(2), // Token có hiệu lực trong 2 giờ
        Issuer = config["JwtSettings:Issuer"],
        Audience = config["JwtSettings:Audience"],
        SigningCredentials = creds
    };

    // 4. Sinh chuỗi Token
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}
```
## IV. Cách cấu hình Validate Token trong Program.cs
Cần cài đặt thư viện NuGet: Microsoft.AspNetCore.Authentication.JwtBearer
Trong Program.cs, bạn đăng ký middleware để ASP.NET Core tự động giải mã và kiểm tra Token cho mỗi request gửi tới:
```C#
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

// 1. Đăng ký dịch vụ Authentication với Bearer Scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,            // Bắt buộc kiểm tra thời gian hết hạn (exp)
        ValidateIssuerSigningKey = true,     // Bắt buộc đối chiếu chữ ký bằng SecretKey
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero           // Triệt tiêu độ trễ 5 phút mặc định của hệ thống
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// 2. Kích hoạt Middleware (BẮT BUỘC: Authentication đứng TRƯỚC Authorization)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
```