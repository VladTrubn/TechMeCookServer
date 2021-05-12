using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechMeCookServer.Models;
using System.Net.Http;
using TechMeCookServer.Services;
using TechMeCookServer.Data;
using System.Web.Http;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace TechMeCookServer.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class RecipesController : Controller
    {

        private readonly IHttpClientService httpClientService;
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext context;

        private String detailUriTemplate = "recipes/{0}/information?apiKey={1}";
        private String randomCollectionUriTemplate = "recipes/random?number={0}&tags={1}&apiKey={2}";
        private String filteredCollectionUriTemplate = "recipes/complexSearch?query={0}&includeIngredients={1}&equipment={2}&diet={3}&type={4}&number={5}&apiKey={6}";

        public RecipesController(IHttpClientService httpClientService, ApplicationDbContext context)
        {
            this.httpClientService = httpClientService;
            this.httpClient = httpClientService.GetHttpClient();
            this.context = context;
        }

        [HttpGet("{recipeId}/information")]
        public async Task<IActionResult> GetDetail(long recipeId, String apiKey)
        {
            var recipe = await this.context.Recipes
                .Include(r => r.comments)
                .Include(r => r.extendedIngredients)
                .Include(r => r.analyzedInstructions)
                .ThenInclude(i => i.steps)
                .SingleOrDefaultAsync(m => m.id == recipeId);
            var responseModel = new Recipe();
            if (recipe == null)
            {
                String actualUri = string.Format(detailUriTemplate, recipeId, httpClientService.GetApiKey());
                var response = await httpClient.GetAsync(actualUri);
                response.EnsureSuccessStatusCode();
                var responseText = await response.Content.ReadAsStringAsync();
                responseModel = JsonSerializer.Deserialize<Recipe>(responseText);

                recipe = new Recipe
                {
                    id = responseModel.id,
                    spoonacularSourceUrl = responseModel.spoonacularSourceUrl,
                    title = responseModel.title,
                    summary = responseModel.summary,
                    readyInMinutes = responseModel.readyInMinutes,
                    image = responseModel.image,
                    comments = new List<Comment>(),
                    analyzedInstructions = responseModel.analyzedInstructions,
                    extendedIngredients = responseModel.extendedIngredients
                };
                this.context.Recipes.Add(recipe);
                await this.context.SaveChangesAsync();
            }

            else
                responseModel = recipe;

            recipe.comments = recipe.comments.OrderBy(c => c.Created).ToList();
            return Ok(responseModel);
        }


        [HttpGet("random")]
        public async Task<IActionResult> GetRandomCollection(int? number, String? tags, String apiKey)
        {
            String actualUri = string.Format(randomCollectionUriTemplate, number, tags, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<RandomRecipeCollection>(responseText);
            foreach (Recipe entry in responseModel.recipes)
            {    
                var recipe = await this.context.Recipes.Include(r => r.comments).Include(r => r.extendedIngredients).Include(r => r.analyzedInstructions).ThenInclude(i => i.steps).SingleOrDefaultAsync(m => m.id == entry.id);
                if (recipe == null)
                {
                    recipe = new Recipe
                    {   
                        id = entry.id,
                        spoonacularSourceUrl = entry.spoonacularSourceUrl,
                        title = entry.title,
                        summary = entry.summary,
                        readyInMinutes = entry.readyInMinutes,
                        image = entry.image,
                        comments = new List<Comment>(),
                        analyzedInstructions = entry.analyzedInstructions,
                        extendedIngredients = entry.extendedIngredients
                    };
                    this.context.Recipes.Add(recipe);
                    await this.context.SaveChangesAsync();
                }
            }
            return Ok(await response.Content.ReadAsStringAsync());
        }

        [HttpGet("complexSearch")]
        public async Task<IActionResult> GetFilteredCollection(String? query, String? includeIngredients, String? equipment, String? diet, String? type, String? number, String apiKey)
        {
            String actualUri = string.Format(filteredCollectionUriTemplate, query, includeIngredients, equipment,  diet, type, number, httpClientService.GetApiKey());
            var response = await httpClient.GetAsync(actualUri);
            var responseText = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<RecipeCollection>(responseText);
            responseModel.recipes = responseModel.results;
            return Ok(responseModel);
        }

    }
}
