using Microsoft.AspNetCore.Mvc;

namespace LyThuyet.ViduHttpRequestMethod.RouteValue
{
    [ApiController]
    [Route("/weathers/routeValue/{summary}/{temperature}")]
    public class Vidu:ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
            
        {
            //  Tạo key
            var summary = HttpContext.Request.RouteValues["summary"];
            var temperature = HttpContext.Request.RouteValues["temperature"];

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
