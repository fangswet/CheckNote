using CheckNote.Server.Services.Extensions;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class CourseService : CheckNoteService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Course> courses;
        private readonly UserManager<User> userManager;
        private readonly HttpContext httpContext;

        public CourseService(ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            courses = dbContext.Courses;
            this.userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ServiceResult<Course>> Get(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound<Course>();

            return Ok(course);
        }

        public async Task<ServiceResult<List<Note>>> GetNotes(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound<List<Note>>();

            return Ok(course.GetNotes());
        }

        public async Task<ServiceResult<List<Question>>> GetQuestions(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound<List<Question>>();

            return Ok(course.GetQuestions());
        }

        public async Task<ServiceResult<int>> Add(CourseModel input)
        {
            Course course = input;

            var user = await userManager.GetUserAsync(httpContext.User);

            course.AuthorId = user.Id;

            var result = await courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return Ok(result.Entity.Id);
        }

        public async Task<ServiceResult> AddNote(int id, int noteId)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var note = await dbContext.Notes.FindAsync(noteId);

            if (note == null) return NotFound();

            course.CourseNotes.Add(new CourseNote { Course = course, Note = note });
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<ServiceResult> Like(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var user = await userManager.GetUserAsync(httpContext.User);

            if (course.Author.Id == user.Id) return BadRequest();

            var courseLike = course.Likes.FirstOrDefault(cl => cl.User.Id == user.Id);

            if (courseLike != null) return BadRequest();

            course.Likes.Add(new CourseLike { Course = course, User = user });
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<ServiceResult<int>> Practice(int id, AnswerAttempt[] answers)
        {
            var course = await courses.FindAsync(id);
            if (course == null) return NotFound<int>();

            return Ok(course.Practice(answers));
        }

        public async Task<ServiceResult<int>> Test(int id, AnswerAttempt[] answers)
        {
            var course = await courses.FindAsync(id);
            if (course == null) return NotFound<int>();

            var result = course.Practice(answers);

            dbContext.TestResults.Add(new TestResult
            {
                Course = course,
                User = await userManager.GetUserAsync(httpContext.User),
                Result = result
            });

            await dbContext.SaveChangesAsync();

            return Ok(result);
        }
    }
}
