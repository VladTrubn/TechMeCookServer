using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechMeCookServer.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using TechMeCookServer.Services;
using System.IO;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> logger;
        private readonly HttpClient httpClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientService httpClientService)
        {
            this.logger = logger;
            this.httpClient = httpClientService.GetHttpClient();
        }

        [HttpGet]
        public async Task<String> Get()
        {
            var response = await httpClient.GetAsync("recipes/639450/information?apiKey=bee92de871cb4b05ac3b51de0cd576c2");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
            /* var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray(); */
        }
    }
}
