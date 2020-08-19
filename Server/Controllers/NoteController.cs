using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CheckNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly DbSet<Note> notes;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;

        public NoteController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            notes = dbContext.Notes;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            NoteModel note = await notes.FirstOrDefaultAsync(n => n.Id == id);

            if (note == null) return NotFound();

            return Ok(note);
        }

        [AllowAnonymous]
        public IActionResult Get() => Ok(notes.Select(n => (NoteModel)n));

        [HttpPost]
        public async Task<IActionResult> Add(NoteModel input)
        {
            Note note = input;
            note.Author = await userManager.GetUserAsync(HttpContext.User);

            if (note.ParentId != null && await notes.FindAsync(note.ParentId) == null) 
                return BadRequest();
            
            NoteModel result = (await notes.AddAsync(note)).Entity;
            await dbContext.SaveChangesAsync();

            return Ok(result);
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Questions(int id)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound();

            return Ok(note.Questions);
        }

        //[HttpDelete]
        //[Route("{id}")]
        //public async Task<IActionResult> Remove(int id)
        //{
        //    var note = await notes.FindAsync(id);

        //    if (note == null) return NotFound();

        //    notes.Remove(note);
        //    await dbContext.SaveChangesAsync();

        //    return Ok();
        //}

        //[HttpPut]
        //[Route("{id}")]
        //public async Task<IActionResult> Update(int id, NoteInput updatedNote)
        //{
        //    var note = await notes.FindAsync(id);

        //    if (note == null) return NotFound();

        //    updatedNote.Parent = null;

        //    notes.Update(updatedNote);
        //    await dbContext.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
