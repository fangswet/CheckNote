using CheckNote.Shared.Models;
using CheckNote.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class AuthenticationService : CheckNoteService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly JwtService jwtService;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtService = jwtService;
        }

        public async Task<ServiceResult> Login(LoginModel input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return Unauthorized();

            var result = await signInManager.PasswordSignInAsync(user, input.Password, false, false);

            return result.Succeeded ? Ok() : Unauthorized();
        }

        public async Task<ServiceResult> Register(RegisterModel input)
        {
            if ((await userManager.FindByEmailAsync(input.Email)) != null || (await userManager.FindByNameAsync(input.UserName)) != null)
                return Conflict("user already exists");

            var user = new User
            {
                Email = input.Email,
                UserName = input.UserName
            };

            await userManager.CreateAsync(user, input.Password);

            return Ok();
        }

        public async Task<ServiceResult> Logout()
        {
            await signInManager.SignOutAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<ServiceResult<string>> Jwt(LoginModel input)
        {
            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return Unauthorized<string>();

            await signInManager.CheckPasswordSignInAsync(user, input.Password, false);

            return Ok(await jwtService.GenerateToken(user));
        }
    }
}
