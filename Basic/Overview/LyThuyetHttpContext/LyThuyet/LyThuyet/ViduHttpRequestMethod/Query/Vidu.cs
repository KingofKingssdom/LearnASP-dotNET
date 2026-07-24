using Microsoft.AspNetCore.Mvc;

namespace LyThuyet.ViduHttpRequestMethod.Query
{
    [ApiController]
    [Route("/weathers/query")]
    public class Vidu: ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()

        {
            //  Tạo key
            var summary = HttpContext.Request.Query["s"];
            var temperature = HttpContext.Request.Query["t"];

            Console.WriteLine($"---> summary: {summary}, temperature: {temperature}");
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
