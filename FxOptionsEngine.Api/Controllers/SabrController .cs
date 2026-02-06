using FxOptionsEngine.Surfaces;
using Microsoft.AspNetCore.Mvc;

namespace FxOptionsEngine.Api.Controllers
{
    [ApiController]
    [Route("api/sabr")]
    public class SabrController : ControllerBase
    {
        private readonly IVolatilitySurface volatilitySurface;

        public SabrController(IVolatilitySurface volatilitySurface)
        {
            this.volatilitySurface = volatilitySurface;
        }

        [HttpGet("volatility")]
        public IActionResult GetVolatility(
            [FromQuery] double strike,
            [FromQuery] double timeToExpiry
        )
        {
            return Ok(new
            {
                Volatility = volatilitySurface.GetVolatility(strike, timeToExpiry) 
            });
        }
    }
}
