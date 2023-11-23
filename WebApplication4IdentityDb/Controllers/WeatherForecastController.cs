using Microsoft.AspNetCore.Mvc;
using WebApplication4IdentityDb.Auth;

namespace WebApplication4IdentityDb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IAuthService _authService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet(Name = "Login")]
        public async Task<IActionResult> Login()
        {
            var login = await _authService.Login(new LoginUser { UserName = "admin", Password = "admin" });
            if (login)
            {
                await _authService.GenerateCookieAuthentication("admin");
            }
            return Ok(login);
        }
    }
}
