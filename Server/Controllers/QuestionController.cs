using System.Threading.Tasks;
using CheckNote.Server.Services;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly QuestionService questionService;

        public QuestionController(QuestionService questionService)
        {
            this.questionService = questionService;
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
            => await questionService.Get(id);

        [HttpPost]
        public async Task<IActionResult> Add(QuestionModel question)
            => await questionService.Add(question);

        [HttpPost]
        [Route("{id:int}")]
        public async Task<IActionResult> Answer(int id, AnswerAttempt attempt)
            => await questionService.Answer(id, attempt);
    }
}
