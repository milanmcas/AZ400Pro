using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lab11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigDemoController : ControllerBase
    {
        private readonly IOptionsSnapshot<MyConfig> _config;

        public ConfigDemoController(IOptionsSnapshot<MyConfig> config)
            => _config = config;

        [HttpGet()]
        public IActionResult Get()
        {
            return Ok(_config.Value);
        }
    }
}
