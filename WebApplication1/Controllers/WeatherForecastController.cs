using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
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
        [AllowAnonymous]
        [Route("CallAPI")]
        [HttpGet]
        public async Task<IActionResult> CallAPI()
        {
            //CALL GET
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

                using (HttpResponseMessage response = await client.GetAsync("todos/1"))
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    response.EnsureSuccessStatusCode();

                    return Ok(responseContent);
                }
            }

        }
        //[AllowAnonymous]
        //[Route("CallAPI")]
        //[HttpGet]
        //public async Task<IActionResult> CallAPI()
        //{
        //    // CALL POST
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        //        var postData = new
        //        {
        //            title = "foo",
        //            body = "bar",
        //            userId = 1
        //        };

        //        var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
        //        using (HttpResponseMessage response = await client.PostAsync("posts", content))
        //        {
        //            var responseContent = response.Content.ReadAsStringAsync().Result;
        //            response.EnsureSuccessStatusCode();

        //            return Ok(responseContent);
        //        }

        //    }
        //}
        //[AllowAnonymous]
        //[Route("CallAPI")]
        //[HttpGet]
        //public async Task<IActionResult> CallAPI()
        //{
        //    // CALL PUT
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        //        var postData = new
        //        {
        //            title = "foo",
        //            body = "bar",
        //            userId = 1,
        //            id = 1
        //        };
        //        var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

        //        using (HttpResponseMessage response = await client.PutAsync("todos/1", content))
        //        {
        //            var responseContent = response.Content.ReadAsStringAsync().Result;
        //            response.EnsureSuccessStatusCode();

        //            return Ok(responseContent);
        //        }

        //    }
        //}
        //[AllowAnonymous]
        //[Route("CallAPI")]
        //[HttpGet]
        //public async Task<IActionResult> CallAPI()
        //{
        //     CALL DELETE
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

        //        using (HttpResponseMessage response = await client.DeleteAsync("posts/1"))
        //        {
        //            var responseContent = response.Content.ReadAsStringAsync().Result;
        //            response.EnsureSuccessStatusCode();

        //            return Ok(responseContent);
        //        }
        //    }
        //}
    }
}