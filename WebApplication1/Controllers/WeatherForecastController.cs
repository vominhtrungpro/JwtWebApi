using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication1.Controllers
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
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("authorized-request")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CallAPIGet()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            //CALL GET
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

                client.BaseAddress = new Uri("https://localhost:7133/");

                using (HttpResponseMessage response = await client.GetAsync("api/Auth/authorized"))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();
                    return Ok(responseContent);
                }
            }
        }

        [AllowAnonymous]
        [Route("login-request")]
        [HttpPost]
        public async Task<IActionResult> CallAPIPost()
        {
            // CALL POST
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7133/");

                var postData = new
                {
                    username = "string",
                    password = "string"
                };

                var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await client.PostAsync("api/Auth/login", content))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();

                    return Ok(responseContent);
                }
            }
        }

        [AllowAnonymous]
        [Route("demo-request")]
        [HttpPut]
        public async Task<IActionResult> CallAPIPut()
        {
            // CALL PUT
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                var postData = new
                {
                    title = "foo",
                    body = "bar",
                    userId = 1,
                    id = 1
                };
                var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PutAsync("todos/1", content))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();

                    return Ok(responseContent);
                }
            }
        }

        [AllowAnonymous]
        [Route("demo-request")]
        [HttpDelete]
        public async Task<IActionResult> CallAPIDelete()
        {
            //CALL DELETE
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                using (HttpResponseMessage response = await client.DeleteAsync("posts/1"))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();

                    return Ok(responseContent);
                }
            }
        }
    }
}