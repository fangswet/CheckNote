using CheckNote.Shared.Models;
using CheckNote.Shared.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class AuthenticationService
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
            var result = new ServiceResult();

            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return result.Unauthorized();

            var login = await signInManager.PasswordSignInAsync(user, input.Password, false, false);

            return login.Succeeded ? result.Ok() : result.Unauthorized();
        }

        public async Task<ServiceResult> Register(RegisterModel input)
        {
            var result = new ServiceResult();

            if ((await userManager.FindByEmailAsync(input.Email)) != null || (await userManager.FindByNameAsync(input.UserName)) != null)
                return result.Conflict("user already exists");

            var user = new User
            {
                Email = input.Email,
                UserName = input.UserName
            };

            await userManager.CreateAsync(user, input.Password);

            return result.Ok();
        }

        public async Task<ServiceResult> Logout()
        {
            await signInManager.SignOutAsync();

            return new ServiceResult().Ok();
        }

        [HttpPost]
        public async Task<ServiceResult<string>> Jwt(LoginModel input)
        {
            var result = new ServiceResult<string>();

            var user = await userManager.FindByEmailAsync(input.Email);
            if (user == null) return result.Unauthorized();

            await signInManager.CheckPasswordSignInAsync(user, input.Password, false);

            return result.Ok(await jwtService.GenerateToken(user));
        }
    }
}
