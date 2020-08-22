using System.Threading.Tasks;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = AuthenticationScheme.All)]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Question> questions;

        public QuestionController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            questions = dbContext.Questions;
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var question = await questions.FindAsync(id);

            if (question == null) return NotFound();

            return Ok((QuestionModel)question);
        }

        [HttpPost]
        public async Task<IActionResult> Add(QuestionModel question)
        {
            if (question.Type == QuestionType.Binary)
            {
                if (question.Correct == null) return BadRequest();
                question.Answers = null;
            }
            else
            {
                if (question.Answers.Count == 0) return BadRequest();
                question.Correct = null;
            }

            await questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Answer(int id, AnswerAttempt attempt)
        {
            var question = await questions.FindAsync(id);

            if (question == null) return NotFound();

            return Ok(question.Answer(attempt));
        }
    }
}
