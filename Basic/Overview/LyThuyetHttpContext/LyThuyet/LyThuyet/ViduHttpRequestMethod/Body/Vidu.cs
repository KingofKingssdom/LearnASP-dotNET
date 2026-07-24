using Microsoft.AspNetCore.Mvc;

namespace LyThuyet.ViduHttpRequestMethod.Body
{
    [ApiController]
    [Route("/weathers/body")]
    public class Vidu: ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                // 2. Đọc toàn bộ Stream từ đầu đến cuối thành một chuỗi string
                string bodyText = await reader.ReadToEndAsync();

                // 3. In ra màn hình console xem thử
                Console.WriteLine($"---> Dữ liệu Body nhận được: {bodyText}");
                return Ok(new { Message = "Đọc Body thành công", RawContent = bodyText });
            }
        }
    }
}
