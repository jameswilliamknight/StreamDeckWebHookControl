using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace StreamDeckControl.API.Controllers
{
    [Route("lights")]
    [ApiController]
    public class LightsController : ControllerBase
    {
        private readonly AppConfig _options;

        public LightsController(IOptions<AppConfig> options)
        {
            _options = options.Value;
        }

        [HttpGet]
        [Route("bedroom")]
        public async Task<ActionResult<string>> BedroomLights(
            [FromQuery(Name = "brightness")] string brightnessRequested)
        {
            if (!int.TryParse(brightnessRequested, out var brightness) || 
                brightness < 0 || brightness > 100)
            {
                return BadRequest("Brightness value must 0-100");
            }

            // https://ifttt.com/maker_webhooks --> "Documentation"
            var iftttKey = _options.IftttKey;

            //var device = ;
            var devices = new []
            {
                "bedroom_door_light",
                "bedroom_window_light"
            };

            foreach (var device in devices)
            {
                var url = new Uri($"https://maker.ifttt.com/trigger/{device}/with/key/{iftttKey}");
                var client = new HttpClient();
                var content = new StringContent($"{{ \"value1\" : \"{brightness}\" }}", Encoding.UTF8, "application/json");
                await client.PostAsync(url, content);
            }

            return Accepted();
        }
    }
}
