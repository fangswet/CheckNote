using CheckNote.Server.Services.Extensions;
using CheckNote.Shared.Models;
using System.Linq;

namespace CheckNote.Server.Extensions
{
    public static class NoteExtensions
    {
        public static int Practice(this Note note, AnswerAttempt[] answers)
        {
            if (note.Questions.Count == 0) return 0;

            var fullScore = note.Questions.Select(q => (int)q.Difficulty).Sum();
            int score = 0;

            foreach (var answer in answers)
            {
                var question = note.Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question != null && question.Answer(answer))
                {
                    score += (int)question.Difficulty;
                }
            }

            return (int)(score / (double)fullScore * 100);
        }
    }
}
