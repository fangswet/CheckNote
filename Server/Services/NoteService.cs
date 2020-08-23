using CheckNote.Server.Extensions;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class NoteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly DbSet<Note> notes;
        private readonly HttpContext httpContext;

        public NoteService(ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            notes = dbContext.Notes;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ServiceResult<Note, NoteModel>> Get(int id)
        {
            var result = new ServiceResult<Note, NoteModel>();

            var note = await notes.FindAsync(id);

            if (note == null) return result.NotFound();

            return result.Ok(note);
        }

        public async Task<ServiceResult<int>> Add(NoteModel input)
        {
            var result = new ServiceResult<int>();

            Note note = input;
            note.Author = await userManager.GetUserAsync(httpContext.User);

            if (note.ParentId != null && await notes.FindAsync(note.ParentId) == null)
                return result.BadRequest("parent does not exist");

            var added = await notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return result.Ok(added.Entity.Id);
        }

        public async Task<ServiceResult<List<Question>>> GetQuestions(int id)
        {
            var result = new ServiceResult<List<Question>>();
            var note = await notes.FindAsync(id);

            if (note == null) return result.NotFound();

            return result.Ok(note.Questions);
        }

        public async Task<ServiceResult<int>> Practice(int id, AnswerAttempt[] answers)
        {
            var result = new ServiceResult<int>();

            var note = await notes.FindAsync(id);

            if (note == null) return result.NotFound();

            return result.Ok(note.Practice(answers));
        }
    }
}
