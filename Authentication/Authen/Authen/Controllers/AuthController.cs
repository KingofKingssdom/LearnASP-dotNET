using Authen.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authen.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return BadRequest(new { Message = "Email đã tồn tại trong hệ thống!" });

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Đăng ký tài khoản thành công!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            // 1. Tìm user theo Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized(new { Message = "Email hoặc mật khẩu không đúng!" });

            // 2. Kiểm tra mật khẩu
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
                return Unauthorized(new { Message = "Email hoặc mật khẩu không đúng!" });

            // 3. Nếu đúng ➔ Sinh JWT Token
            var token = GenerateJwtToken(user);

            // 4. Trả Token về cho Client
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            // 1. Khai báo các thông tin (Claims) muốn nhúng vào Token
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Email, user.Email!),
        //new Claim(ClaimTypes.Role, user.Role.ToString()) // Nhúng Enum Role của bạn vào đây!
    };

            // 2. Lấy Secret Key từ appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Cấu hình Token (Thời gian hết hạn, Người phát hành...)
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // Token có hiệu lực 2 tiếng
                signingCredentials: creds
            );

            // 4. Convert thành chuỗi string "eyJhbGciOi..."
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
