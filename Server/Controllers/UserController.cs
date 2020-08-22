using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthenticationScheme.All)]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public UserController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Get() => Ok(userManager.Users.Select(u => (UserModel)u));

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            UserModel user = await userManager.FindByIdAsync(id.ToString());

            return Ok(user);
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Notes(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            return Ok(user.Notes.Select(n => (NoteModel)n));
        }

        [Route("[action]")]
        public async Task<IActionResult> Me()
        {
            UserModel me = await userManager.GetUserAsync(HttpContext.User);

            return Ok(me);
        }

        [Authorize(Roles = Role.Admin)]
        [Route("[action]/{id:int}")]
        public async Task<IActionResult> Elevate(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user == null) return NotFound();

            await userManager.AddToRoleAsync(user, Role.Admin);

            return Ok();
        }
    }
}
