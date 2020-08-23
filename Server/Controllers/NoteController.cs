using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CheckNote.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly NoteService noteService;
        private readonly DbSet<Note> notes;

        public NoteController(NoteService noteService, ApplicationDbContext dbContext)
        {
            this.noteService = noteService;
            notes = dbContext.Notes;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Get() 
            => Ok(await notes.ToListAsync());

        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var serviceResult = await noteService.Get(id);

            var sr = serviceResult.Sanitize();
            var ar = Ok(serviceResult.Sanitize().Value);

            return ar;
        }

        [HttpPost]
        public async Task<IActionResult> Add(NoteModel input) 
            => await noteService.Add(input);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Questions(int id)
            => await noteService.GetQuestions(id);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Practice(int id, AnswerAttempt[] answers)
            => await noteService.Practice(id, answers);

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
