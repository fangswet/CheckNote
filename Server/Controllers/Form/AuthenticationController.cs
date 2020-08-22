using System.Threading.Tasks;
using CheckNote.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Controllers.Form
{
    [Route("form/[action]")]
    public class AuthenticationController : FormController
    {
        private readonly AuthenticationService authService;

        public AuthenticationController(AuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string username, string password)
        {

            return Back();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {


            return Back();
        }

        public async Task<IActionResult> Logout()
        {
            await authService.Logout();

            return Back();
        }
    }
}
