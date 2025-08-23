using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using AWSsnsApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AWSsnsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        
        [HttpPost("publish")]
        public async Task<IActionResult> Publish([FromBody] SnsMessage message)
        {
            var request = new PublishRequest
            {
                TopicArn = message.TopicArn,
                Message = message.Message
            };

            var response = await _snsClient.PublishAsync(request);

            return Ok(new
            {
                MessageId = response.MessageId,
                Status = response.HttpStatusCode
            });
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAmazonSimpleNotificationService snsClient)
        {
            _logger = logger;
            _snsClient = snsClient;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
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
