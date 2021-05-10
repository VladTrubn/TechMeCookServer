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
using Microsoft.AspNetCore.Http;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {

        private readonly IHttpClientService httpClientService;
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(UserManager<ApplicationUser> userManager, IHttpClientService httpClientService, ApplicationDbContext context)
        {
            this.httpClientService = httpClientService;
            this.httpClient = httpClientService.GetHttpClient();
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetComments(long recipeId, String apiKey)
        {

            var recipe = await this.context.Recipes.Include(r => r.comments).SingleOrDefaultAsync(m => m.id == recipeId);

            if (recipe==null)
            {
                throw new HttpRequestException("No such recipe in DB");
            }
            
            var comments = recipe.comments;
            String jsonResponse = JsonSerializer.Serialize(comments);

            return Ok(jsonResponse);
            
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateComment()
        {
            var stream = new StreamReader(HttpContext.Request.Body);
            var body = await stream.ReadToEndAsync();
            var requestBody = JsonSerializer.Deserialize<Comment>(body);

            try
            {
                var comment = new Comment
                {
                    Text = requestBody.Text,
                    CreatorId = await this.userManager.GetUserIdAsync(await this.userManager.FindByEmailAsync(requestBody.CreatorId)),
                    //Creator = await userManager.FindByIdAsync(requestBody.CreatorId),
                    Created = DateTime.UtcNow,
                    RecipeId = this.context.Recipes.SingleOrDefault(r => r.id.ToString() == requestBody.RecipeDbId).RId,
                    RecipeDbId = requestBody.RecipeDbId,
                    //Recipe = await this.context.Recipes.SingleOrDefaultAsync(r => r.RId == requestBody.RecipeId)
                };


                this.context.Comments.Add(comment);
                await this.context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateComment), comment);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }



    }
}
