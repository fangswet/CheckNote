using System.Diagnostics;
using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ApplicationController
    {
        private readonly DbSet<Note> notes;

        public NoteController(ApplicationDbContext dbContext, UserManager<User> userManager) : base(dbContext, userManager)
        {
            notes = dbContext.Notes;
        }

        [AllowAnonymous]
        [Route("{id?}")]
        public async Task<IActionResult> Get(int id)
        {
            NoteOutput note = await notes
                .Include(n => n.Author)
                .Include(n => n.Children)
                .FirstOrDefaultAsync(n => n.Id == id);
            
            return Serve(note);
        }

        [HttpPost]
        public async Task<IActionResult> Add(NoteInput input)
        {
            Note note = input;

            note.Author = await GetUser();

            if (await notes.FindAsync(input.Parent) == null) return BadRequest();

            await notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound();

            notes.Remove(note);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, NoteInput updatedNote)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound();

            updatedNote.Parent = null;

            notes.Update(updatedNote);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
