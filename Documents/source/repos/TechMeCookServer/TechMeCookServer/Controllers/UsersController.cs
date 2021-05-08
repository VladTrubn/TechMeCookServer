using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TechMeCookServer.Models;
using System.Net.Http;
using TechMeCookServer.Services;
using TechMeCookServer.Data;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Web.Http;
using System.Net.Http.Formatting;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ApiController
    {
        private readonly ILogger<RecipesController> logger;
        private readonly IHttpClientService httpClientService;
        private readonly HttpClient httpClient;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<RecipesController> logger, IHttpClientService httpClientService,
           ApplicationDbContext context)
        {
            this.logger = logger;
            this.httpClientService = httpClientService;
            this.httpClient = httpClientService.GetHttpClient();
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("login")]
        public async Task<ApplicationUser> Login(String email, String password)
        {

            var User = await userManager.FindByEmailAsync(email);
            if (User == null)
            {
                throw new HttpRequestException("No such user");
            }

            var result = await this.signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new HttpRequestException("Wrong password");
            }

            return User;       
        }

        [HttpGet("register")]
        public async Task<ApplicationUser> Register(String username, String email, String password)
        {
            var user = new ApplicationUser { UserName = username, Email = email };
            var result = await this.userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return await userManager.FindByEmailAsync(email);
            }

            throw new HttpRequestException("Such a user already exists");
        }


    }
}
