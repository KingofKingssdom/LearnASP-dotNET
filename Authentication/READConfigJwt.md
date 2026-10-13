# Cấu hình Jwt trong ASP .NET Core

**Bước 1: Cài đặt package cần thiết** <br/>
Chạy lệnh sau để cài đặt package cho JWT
``` C# 
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

**Bước 2: Cấu hình thông số JWT** <br/>
Trong file appsettings.json, bạn có thể thêm các thông số cho JWT:

``` C#
{
  "Jwt": {
    "Key": "SecretKeyToGenerateJWTToken1234567890", // Key cần giữ bí mật
    "Issuer": "MyAuthServer",                      // Nhà cung cấp token
    "Audience": "MyApiClient",                     // Đối tượng sử dụng token
    "DurationInMinutes": 60                        // Thời gian sống của token
  }
}
```
**Hiểu thông số:**
Issuer (Người cấp thẻ - "Ai ban hành?")
+ Ý nghĩa: Là tên/định danh của Authentication Server (Server đứng ra làm nhiệm vụ Login và cấp Token). <br/>
Ví dụ thực tế: Giống như trên Căn cước công dân của bạn có ghi "Nơi cấp: Cục Cảnh sát Quản lý Hành chính".
+ Tác dụng trong Code: Khi API Server nhận được Token từ Client gửi lên, nó sẽ kiểm tra dòng Issuer này. Nếu Issuer trong Token không khớp với giá trị cấu hình trong appsettings.json, Server sẽ ngay lập tức từ chối vì nghi ngờ Token này do một Server lạ/mạo danh phát hành. <br/>
**Hiểu thông số:**
Audience (Đối tượng sử dụng - "Thẻ này dành cho ai?")
+ Ý nghĩa: Là tên/định danh của Client hoặc Service được phép sử dụng Token này để truy cập tài nguyên.<br/>
Ví dụ thực tế: Bạn mua vé xem phim, trên vé ghi rõ "Dành cho: Rạp CGV Landmark 81". Bạn không thể mang vé đó sang rạp Lotte Cinema để xem được. <br/>
Tác dụng trong Code: Giúp bảo vệ hệ thống trong mô hình Microservices hoặc khi bạn có nhiều app khác nhau (như Web Admin, Mobile App, Partner App). Server sẽ kiểm tra xem Token này tạo ra có phải để dành riêng cho hệ thống/app của mình dùng hay không.

**Bước 3: Cấu hình JWT Authentication trong ứng dụng** <br/>
Trong file Program.cs (cho ASP.NET Core 6 trở lên), cấu hình authentication như sau:

``` C#
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Đọc thông số JWT từ cấu hình
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

// Cấu hình Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Chỉ dùng trong dev, nên bật true ở production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

// Đăng ký Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```

**Bước 4: Tạo API Controller để phát hành JWT** <br/>
Dưới đây là ví dụ về một controller xác thực người dùng và trả về token JWT khi đăng nhập thành công

``` C# 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // Ví dụ đăng nhập (Login) - trong thực tế cần kiểm tra thông tin người dùng từ database
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Kiểm tra thông tin đăng nhập (ở đây giả định luôn thành công)
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = GenerateJwtToken(request.Username);
            return Ok(new { Token = token });
        }
        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                // Thêm các claim khác nếu cần, ví dụ: role, email,...
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
```

**Bước 5: Bảo vệ các API bằng cách sử dụng Authorize Attribute**

Sau khi cấu hình authentication, bạn có thể bảo vệ các controller hoặc action bằng cách sử dụng [Authorize]

``` C#
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize] // Yêu cầu phải có token hợp lệ để truy cập
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        // Chỉ những request đã được xác thực mới truy cập được endpoint này
        return Ok(new[] { "Product1", "Product2" });
    }
}
```