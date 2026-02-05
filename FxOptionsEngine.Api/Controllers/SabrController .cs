using Microsoft.AspNetCore.Mvc;

namespace FxOptionsEngine.Api.Controllers
{
    [ApiController]
    [Route("api/sabr")]
    public class SabrController : ControllerBase
    {
        [HttpGet("volatility")]
        public IActionResult GetVolatility(
            [FromQuery] double forward,
            [FromQuery] double strike
        )
        {

            return Ok(new
            {
                Forward = forward,
                Strike = strike,
                Volatility = 0.085 
            });
        }
    }
}
