using CheckNote.Server.Services.Extensions;
using CheckNote.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class QuestionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<Question> questions;

        public QuestionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            questions = dbContext.Questions;
        }

        public async Task<ServiceResult<Question, QuestionModel>> Get(int id)
        {
            var result = new ServiceResult<Question, QuestionModel>();

            var question = await questions.FindAsync(id);

            if (question == null) return result.NotFound();

            return result.Ok(question);
        }

        public async Task<ServiceResult<int>> Add(QuestionModel question)
        {
            var result = new ServiceResult<int>();

            if (question.Type == QuestionType.Binary)
            {
                if (question.Correct == null) return result.BadRequest();
                question.Answers = null;
            }
            else
            {
                if (question.Answers.Count == 0) return result.BadRequest();
                question.Correct = null;
            }

            var added = await questions.AddAsync(question);
            await dbContext.SaveChangesAsync();

            return result.Ok(added.Entity.Id);
        }

        public async Task<ServiceResult<bool>> Answer(int id, AnswerAttempt attempt)
        {
            var result = new ServiceResult<bool>();

            var question = await questions.FindAsync(id);

            if (question == null) return result.NotFound();

            return result.Ok(question.Answer(attempt));
        }
    }
}
