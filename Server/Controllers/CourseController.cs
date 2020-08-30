using CheckNote.Server.Services;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly CourseService courseService;
        private readonly DbSet<Course> courses;

        public CourseController(CourseService courseService, ApplicationDbContext dbContext)
        {
            this.courseService = courseService;
            courses = dbContext.Courses;
        }

        public async Task<IActionResult> Get() => Ok(await courses.Select(s => (CourseModel)s).ToListAsync());

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id) => (await courseService.Get(id)).Result();

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Notes(int id) => (await courseService.GetNotes(id)).Result();

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Questions(int id) => (await courseService.GetQuestions(id)).Result();

        [HttpPost]
        public async Task<IActionResult> Add(CourseModel input) => (await courseService.Add(input)).Result();

        [Route("{id:int}/[action]/{noteId:int}")]
        public async Task<IActionResult> AddNote(int id, int noteId) => (await courseService.AddNote(id, noteId)).Result();

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Like(int id) => (await courseService.Like(id)).Result();

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Practice(int id, AnswerAttempt[] answers) => (await courseService.Practice(id, answers)).Result();

        [Route("{id:int}/[action]")]
        public async Task<IActionResult> Test(int id, AnswerAttempt[] answers) => (await courseService.Test(id, answers)).Result();
    }
}
