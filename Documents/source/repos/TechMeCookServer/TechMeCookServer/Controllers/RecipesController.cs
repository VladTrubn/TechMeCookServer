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
using System.Net.Http.Headers;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController : Controller
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
        private String filteredCollectionUriTemplate = "recipes/complexSearch?query={0}&includeIngredients={1}&equipment={2}&diet={3}&intolerances={4}&number={5}&apiKey={6}";

        public RecipesController(UserManager<ApplicationUser> userManager, ILogger<RecipesController> logger, IHttpClientService httpClientService, ApplicationDbContext context)
        {
            this.logger = logger;
            this.httpClientService = httpClientService;
            this.httpClient = httpClientService.GetHttpClient();
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("{recipeId}/information")]
        public async Task<IActionResult> GetDetail(long recipeId, String apiKey)
        {
            String actualUri = string.Format(detailUriTemplate, recipeId, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);

            var responseText = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<Recipe>(responseText);

            var recipe = await this.context.Recipes.Include(r => r.comments).SingleOrDefaultAsync(m => m.id == recipeId);


            if (recipe == null)
            {
                recipe = new Recipe
                {
                    id = responseModel.id,
                    spoonacularSourceUrl = responseModel.spoonacularSourceUrl,
                    title = responseModel.title,
                    summary = responseModel.summary,
                    readyInMinutes = responseModel.readyInMinutes,
                    image = responseModel.image,
                    comments = new List<Comment>()
                };
                this.context.Recipes.Add(recipe);
                await this.context.SaveChangesAsync();
            }
            String jsonResponse = JsonSerializer.Serialize(recipe);

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.EnsureSuccessStatusCode();
            return Ok(jsonResponse);
        }


        [HttpGet("random")]
        public async Task<IActionResult> GetRandomCollection(int? number, String? tags, String apiKey)
        {
            String actualUri = string.Format(randomCollectionUriTemplate, number, tags, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.EnsureSuccessStatusCode();
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet("complexSearch")]
        public async Task<IActionResult> GetFilteredCollection(String? query, String? includeIngredients, String? equipment, String? diet, String? intolerances, String? number, String apiKey)
        {
            String actualUri = string.Format(filteredCollectionUriTemplate, query, includeIngredients, equipment,  diet, intolerances, number, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);
            var responseText = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<RecipeCollection>(responseText);
            responseModel.recipes = responseModel.results;
            return Ok(responseModel);
        }

    }
}
