using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("[controller]/{id:int}")]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Question> questions;

        public QuestionController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.questions = dbContext.Questions;
        }

        public async Task<IActionResult> Get(int id)
        {
            QuestionModel question = await questions.FindAsync(id);

            if (question == null) return NotFound();

            return Ok(question);
        }

        //[HttpPost]
        //public async Task<IActionResult> Answer(int id)
        //{
        //    QuestionModel question = await questions.FindAsync(id);

        //    if (question == null) return NotFound();

        //    return Ok(question);
        //}
    }
}
