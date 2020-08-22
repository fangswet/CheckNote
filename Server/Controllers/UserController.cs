using CheckNote.Server.Services;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => await userService.Get(id);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Notes(int id)
            => await userService.Notes(id);

        [Route("[action]")]
        public async Task<IActionResult> Me()
            => await userService.Me();

        [Authorize(Roles = Role.Admin)]
        [Route("[action]/{id:int}")]
        public async Task<IActionResult> Elevate(int id)
            => await userService.Elevate(id);
    }
}
