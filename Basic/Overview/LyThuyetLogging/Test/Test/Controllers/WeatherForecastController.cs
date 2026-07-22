using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers
{   
    /*Ví dụ sử dụng messageLogger
     - Sử dụng ILogger<Class/File mà mình muốn theo dõi thay đổi hoăc thông tin trong class này> _logger
     - Sử dụng cơ chế tiêm phụ thuộc vào constructor để có thể lấy được thông tin vào đó sài
     - Trong hàm nào muốn ghi log thì chỉ cần gọi biến đó đến các phuognw thức log 
     */
    [ApiController]
    [Route("/weathers")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];
        private readonly ILogger<WeatherForecastController> _logger;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Thong tin ve logger");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
