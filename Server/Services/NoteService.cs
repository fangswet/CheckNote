using CheckNote.Server.Extensions;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class NoteService : CheckNoteService
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

        public async Task<ServiceResult<Note>> Get(int id)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound<Note>();

            return Ok(note);
        }

        public async Task<ServiceResult<int>> Add(NoteModel input)
        {
            Note note = input;
            note.Author = await userManager.GetUserAsync(httpContext.User);

            if (note.ParentId != null && await notes.FindAsync(note.ParentId) == null)
                return BadRequest<int>("parent does not exist");

            var result = await notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            return Ok(result.Entity.Id);
        }

        public async Task<ServiceResult<List<Question>>> GetQuestions(int id)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound<List<Question>>();

            return Ok(note.Questions);
        }

        public async Task<ServiceResult<int>> Practice(int id, AnswerAttempt[] answers)
        {
            var note = await notes.FindAsync(id);

            if (note == null) return NotFound<int>();

            return Ok(note.Practice(answers));
        }
    }
}
