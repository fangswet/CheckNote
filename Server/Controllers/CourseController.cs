using System.Linq;
using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly DbSet<Course> courses;

        public CourseController(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            courses = dbContext.Courses;
        }

        public async Task<IActionResult> Get()
        {
            var allCourses = await courses.Select(s => (CourseModel)s).ToListAsync();

            return Ok(allCourses);
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            return Ok((CourseModel)course);
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Notes(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var notes = course.CourseNotes.Select(cn => (NoteModel)cn.Note);

            return Ok(notes);
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Questions(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var questions = course.Questions().Select(q => (QuestionModel)q);

            return Ok(questions);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CourseModel input)
        {
            Course course = input;

            var user = await userManager.GetUserAsync(HttpContext.User);

            course.AuthorId = user.Id;

            var result = await courses.AddAsync(course);
            await dbContext.SaveChangesAsync();

            return Ok(result.Entity.Id);
        }

        [Route("{id:int}/[action]/{noteId:int}")]
        public async Task<IActionResult> AddNote(int id, int noteId)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var note = await dbContext.Notes.FindAsync(noteId);

            if (note == null) return NotFound();

            course.CourseNotes.Add(new CourseNote { Course = course, Note = note });
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Like(int id)
        {
            var course = await courses.FindAsync(id);

            if (course == null) return NotFound();

            var user = await userManager.GetUserAsync(HttpContext.User);

            if (course.Author.Id == user.Id) return BadRequest();

            var courseLike = course.Likes.FirstOrDefault(cl => cl.User.Id == user.Id);

            if (courseLike != null) return BadRequest();

            course.Likes.Add(new CourseLike { Course = course, User = user });
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Practice(int id, AnswerAttempt[] answers)
        {
            var course = await courses.FindAsync(id);
            if (course == null) return NotFound();

            return Ok(course.Test(answers));
        }

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Test(int id, AnswerAttempt[] answers)
        {
            var course = await courses.FindAsync(id);
            if (course == null) return NotFound();

            var result = course.Test(answers);

            dbContext.TestResults.Add(new TestResult
            { 
                Course = course,
                User = await userManager.GetUserAsync(HttpContext.User),
                Result = result
            });

            return Ok(result);
        }
    }
}
