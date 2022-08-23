using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherAplication.Queries;

namespace WeatherAplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadWeather()
        {
            var query = new GetWeatherQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
