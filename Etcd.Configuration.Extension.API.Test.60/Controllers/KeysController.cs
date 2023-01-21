using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Etcd.Configuration.Extension.API.Test._60.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeysController : ControllerBase
    {

        private readonly ILogger<KeysController> _logger;
        private readonly IConfiguration configuration;

        public KeysController(ILogger<KeysController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("/fullKey")]
        public IActionResult Get()
        {
            return Ok(configuration.GetSection("testapplication/test").AsEnumerable()
                .Concat(configuration.GetSection("testapplication/testjson").AsEnumerable()));
        }

        [HttpGet("/allKey")]
        public IActionResult GetOnly()
        {
            return Ok(configuration.AsEnumerable());
        }
    }
}