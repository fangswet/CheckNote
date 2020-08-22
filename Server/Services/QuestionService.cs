using CheckNote.Server.Services.Extensions;
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class QuestionService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<User> userManager;
        private readonly DbSet<Question> questions;

        public QuestionService(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
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
