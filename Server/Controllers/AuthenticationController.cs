using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Controllers
{
    [Route("api/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AuthenticationController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Test([FromForm] string message)
        {
            return Ok($"recieved {message}");
        }
    }
}
