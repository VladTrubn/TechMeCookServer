using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechMeCookServer.Models;
using System.Net.Http;
using TechMeCookServer.Services;
using TechMeCookServer.Data;
using Microsoft.AspNetCore.Identity;
using System.Web.Http;
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
            CommentCollection commentCollection = new CommentCollection();

            foreach (Comment comment in comments)
            {
                var user = await userManager.FindByIdAsync(comment.CreatorId);
                comment.CreatorName = user.UserName;
                commentCollection.comments.Add(comment);
            }
            String jsonResponse = JsonSerializer.Serialize(commentCollection);

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

            {
                var comment = new Comment
                {
                    Text = requestBody.Text,
                    CreatorId = await this.userManager.GetUserIdAsync(await this.userManager.FindByEmailAsync(requestBody.CreatorId)),
                    Created = DateTime.UtcNow,
                    RecipeId = this.context.Recipes.SingleOrDefault(r => r.id.ToString() == requestBody.RecipeDbId).RId,
                    RecipeDbId = requestBody.RecipeDbId,
                };


                this.context.Comments.Add(comment);
                await this.context.SaveChangesAsync();
                return CreatedAtAction(nameof(CreateComment), comment);
            }
        }



    }
}
