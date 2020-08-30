using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class UserService
    {
        private readonly UserManager<User> userManager;
        private readonly HttpContext httpContext;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ServiceResult<User, UserModel>> Get(int id)
        {
            var result = new ServiceResult<User, UserModel>();

            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return result.NotFound();

            return result.Ok(user);
        }

        public async Task<ServiceResult<List<Note>>> GetNotes(int id)
        {
            var result = new ServiceResult<List<Note>>();

            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return result.NotFound();

            return result.Ok(user.Notes);
        }

        public async Task<ServiceResult<User, UserModel>> Me()
            => new ServiceResult<User, UserModel>().Ok(await userManager.GetUserAsync(httpContext.User));

        public async Task<ServiceResult> Elevate(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return ServiceResult.NotFound();

            await userManager.AddToRoleAsync(user, Role.Admin);

            return ServiceResult.Ok();
        }
    }
}
