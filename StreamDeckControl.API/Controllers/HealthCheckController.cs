using Microsoft.AspNetCore.Mvc;

namespace StreamDeckWebHookControl.Controllers
{
    public class HealthCheckController
    {
        [HttpGet]
        [Route("/ping")]
        public ActionResult<string> Ping() => "Pong";
    }
}
