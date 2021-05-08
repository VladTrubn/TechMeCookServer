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
using TechMeCookServer.Data;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Web.Http;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : ApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<RecipesController> logger;
        private readonly IHttpClientService httpClientService;
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        private String detailUriTemplate = "recipes/{0}/information?apiKey={1}";
        private String randomCollectionUriTemplate = "recipes/random?number={0}&tags={1}&apiKey={2}";

        public RecipesController(UserManager<ApplicationUser> userManager, ILogger<RecipesController> logger, IHttpClientService httpClientService, ApplicationDbContext context)
        {
            this.logger = logger;
            this.httpClientService = httpClientService;
            this.httpClient = httpClientService.GetHttpClient();
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("{recipeId}/information")]
        public async Task<String> GetDetail(long recipeId, String apiKey)
        {
            String actualUri = string.Format(detailUriTemplate, recipeId, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
            
          /*  var basereturn =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray();
            return basereturn;*/
        }


        [HttpGet("random")]
        public async Task<String> GetRandomCollection(int? number, String? tags, String apiKey)
        {
            String actualUri = string.Format(randomCollectionUriTemplate, number, tags, httpClientService.GetApiKey());
            return actualUri;
            var response = await httpClient.GetAsync(actualUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

    }
}
