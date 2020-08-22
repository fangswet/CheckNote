using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class UserService : CheckNoteService
    {
        private readonly UserManager<User> userManager;
        private readonly HttpContext httpContext;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ServiceResult<User>> Get(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound<User>();

            return Ok(user);
        }

        public async Task<ServiceResult<List<Note>>> Notes(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound<List<Note>>();

            return Ok(user.Notes);
        }

        public async Task<ServiceResult<User>> Me()
            => Ok(await userManager.GetUserAsync(httpContext.User));

        public async Task<ServiceResult> Elevate(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            await userManager.AddToRoleAsync(user, Role.Admin);

            return Ok();
        }
    }
}
