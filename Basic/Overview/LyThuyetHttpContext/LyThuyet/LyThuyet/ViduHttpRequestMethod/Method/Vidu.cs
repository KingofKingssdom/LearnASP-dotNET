using Microsoft.AspNetCore.Mvc;

namespace LyThuyet.ViduHttpRequestMethod.HttpRequestMethod
{
    [ApiController]
    [Route("/weathers/method")]
    public class Vidu: ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine("Phuong thuc gui toi " + HttpContext.Request.Method);
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
