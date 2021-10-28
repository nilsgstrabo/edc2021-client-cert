using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching", "Nice"
        };

        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Data> Get()
        {
            _logger.LogInformation(0, "********* Logging Headers **********");
            _logger.LogInformation(
                0,
                this.Request.Headers.ToList().Where(h=>h.Key.StartsWith("ssl-client-")).Aggregate("", (s, h) =>
                {
                    if (h.Key == "ssl-client-cert")
                    {
                        var unescapedString = Uri.UnescapeDataString(h.Value);
                        var unescapedBytes = System.Text.Encoding.UTF8.GetBytes(unescapedString);
                        var unescapedBase64 = Convert.ToBase64String(unescapedBytes);
                        _logger.LogInformation(0, "ssl-client-cert unescaped:" + unescapedBase64);

                        var escapedBytes = System.Text.Encoding.UTF8.GetBytes(h.Value);
                        var escapedBase64 = Convert.ToBase64String(escapedBytes);
                        _logger.LogInformation(0, "ssl-client-cert original:" + escapedBase64);
                    }
                    return s + h.Key + ": " + h.Value.ToString() + Environment.NewLine;
                })
            );
            _logger.LogInformation(0, "********* Done **********");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Data
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
