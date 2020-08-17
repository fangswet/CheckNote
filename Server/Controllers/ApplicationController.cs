using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    public abstract class ApplicationController : ControllerBase
    {
        protected readonly ApplicationDbContext dbContext;
        protected readonly UserManager<User> userManager;

        public ApplicationController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        protected IActionResult Serve(object value)
        {
            if (value == null) return NotFound();

            return Ok(value);
        }

        protected async Task<User> GetUser() => await userManager.GetUserAsync(HttpContext.User);
    }
}
