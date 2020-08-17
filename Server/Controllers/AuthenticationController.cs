using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CheckNote.Shared.Models;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CheckNote.Server.Services;
using System.Runtime.InteropServices.WindowsRuntime;

namespace CheckNote.Server.Controllers
{
    [Route("[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly JwtService jwt;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwt)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwt = jwt;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                return BadRequest();

            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded) return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string username, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                return BadRequest();

            if ((await userManager.FindByEmailAsync(email)) != null || (await userManager.FindByNameAsync(username)) != null)
                return Conflict();

            var user = new User
            {
                Email = email,
                UserName = username
            };

            await userManager.CreateAsync(user, password);

            //return RedirectToAction("Login", new { user, password });
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Jwt(string email, string password)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password)) 
                return BadRequest();

            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

                if (result.Succeeded)
                {
                    return Ok(await jwt.GenerateToken(user));
                }
            }

            return Unauthorized();
        }

        [Authorize]
        public async Task<IActionResult> Me()
        {
            return Ok(await userManager.GetUserAsync(HttpContext.User));
        }
    }
}
