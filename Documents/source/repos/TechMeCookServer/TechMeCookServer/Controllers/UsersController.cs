using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechMeCookServer.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using System.Web.Http;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace TechMeCookServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;


        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ApplicationUser> Login()
        {
            var stream = new StreamReader(HttpContext.Request.Body);
            var body = await stream.ReadToEndAsync();
            var requestBody = JsonSerializer.Deserialize<LoginBody>(body);

            var User = await userManager.FindByEmailAsync(requestBody.email);
            if (User == null)
            {
                throw new HttpRequestException("No such user");
            }

            var result = await this.signInManager.PasswordSignInAsync(User.UserName, requestBody.password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new HttpRequestException("Wrong password");
            }

            return User;
        }

        [HttpPost("register")]
        public async Task<ApplicationUser> Register()
        {
            var stream = new StreamReader(HttpContext.Request.Body);
            var body = await stream.ReadToEndAsync();
            var requestBody = JsonSerializer.Deserialize<RegisterBody>(body);

            var user = new ApplicationUser { UserName = requestBody.username, Email = requestBody.email };
            var result = await this.userManager.CreateAsync(user, requestBody.password);


            if (result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return await userManager.FindByEmailAsync(requestBody.email);
            }

            String errors = "";
            foreach (var error in result.Errors)
                errors += error.Description;
            throw new HttpRequestException(errors);
        }


    }
}
