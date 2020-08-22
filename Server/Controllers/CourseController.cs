using System.Linq;
using System.Threading.Tasks;
using CheckNote.Server.Services;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthenticationScheme.All)]
    public class CourseController : ControllerBase
    {
        private readonly CourseService courseService;
        private readonly DbSet<Course> courses;

        public CourseController(CourseService courseService, ApplicationDbContext dbContext)
        {
            this.courseService = courseService;
            courses = dbContext.Courses;
        }

        public async Task<IActionResult> Get()
            => Ok(await courses.Select(s => (CourseModel)s).ToListAsync());

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => await courseService.Get(id);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Notes(int id)
            => await courseService.GetNotes(id);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Questions(int id)
            => await courseService.GetQuestions(id);

        [HttpPost]
        public async Task<IActionResult> Add(CourseModel input)
            => await courseService.Add(input);

        [Route("{id:int}/[action]/{noteId:int}")]
        public async Task<IActionResult> AddNote(int id, int noteId)
            => await courseService.AddNote(id, noteId);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Like(int id)
            => await courseService.Like(id);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Practice(int id, AnswerAttempt[] answers)
            => await courseService.Practice(id, answers);

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Test(int id, AnswerAttempt[] answers)
            => await courseService.Test(id, answers);
    }
}
