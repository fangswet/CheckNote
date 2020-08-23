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
    public class CourseService
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

        public async Task<ServiceResult<Course, CourseModel>> Get(int id)
        {
            var result = new ServiceResult<Course, CourseModel>();

            var course = await courses.FindAsync(id);

            if (course == null) return result.NotFound();

            return result.Ok(course);
        }

        public async Task<ServiceResult<List<Note>>> GetNotes(int id)
        {
            var result = new ServiceResult<List<Note>>();

            var course = await courses.FindAsync(id);

            if (course == null) return result.NotFound();

            return result.Ok(course.GetNotes());
        }

        public async Task<ServiceResult<List<Question>>> GetQuestions(int id)
        {
            var result = new ServiceResult<List<Question>>();

            var course = await courses.FindAsync(id);

            if (course == null) return result.NotFound();

            return result.Ok(course.GetQuestions());
        }

        public async Task<ServiceResult<int>> Add(CourseModel input)
        {
            Course course = input;

            var user = await userManager.GetUserAsync(httpContext.User);

            course.AuthorId = user.Id;

            var added = await courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return new ServiceResult<int>().Ok(added.Entity.Id);
        }

        public async Task<ServiceResult> AddNote(int id, int noteId)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return ServiceResult.NotFound();

            var note = await dbContext.Notes.FindAsync(noteId);

            if (note == null) return ServiceResult.NotFound();

            course.CourseNotes.Add(new CourseNote { Course = course, Note = note });
            await dbContext.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult> Like(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return ServiceResult.NotFound();

            var user = await userManager.GetUserAsync(httpContext.User);

            if (course.Author.Id == user.Id) return ServiceResult.BadRequest();

            var courseLike = course.Likes.FirstOrDefault(cl => cl.User.Id == user.Id);

            if (courseLike != null) return ServiceResult.BadRequest();

            course.Likes.Add(new CourseLike { Course = course, User = user });
            await dbContext.SaveChangesAsync();

            return ServiceResult.Ok();
        }

        public async Task<ServiceResult<int>> Practice(int id, AnswerAttempt[] answers)
        {
            var result = new ServiceResult<int>();

            var course = await courses.FindAsync(id);
            if (course == null) return result.NotFound();

            return result.Ok(course.Practice(answers));
        }

        public async Task<ServiceResult<int>> Test(int id, AnswerAttempt[] answers)
        {
            var result = new ServiceResult<int>();

            var course = await courses.FindAsync(id);
            if (course == null) return result.NotFound();

            var score = course.Practice(answers);

            dbContext.TestResults.Add(new TestResult
            {
                Course = course,
                User = await userManager.GetUserAsync(httpContext.User),
                Result = score
            });

            await dbContext.SaveChangesAsync();

            return result.Ok(score);
        }
    }
}
