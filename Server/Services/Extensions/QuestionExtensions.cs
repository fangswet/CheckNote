using CheckNote.Shared.Models;
using System;
using System.Linq;

namespace CheckNote.Server.Services.Extensions
{
    public static class QuestionExtensions
    {
        public static bool Answer(this Question question, AnswerAttempt attempt)
        {
            switch (question.Type)
            {
                case QuestionType.Binary:
                    return attempt.Correct != null && question.Correct == attempt.Correct;
                case QuestionType.Single:
                case QuestionType.Multiple:

                    if (attempt.CorrectAnswers == null) return false;

                    var correctAnswers = question.Answers
                        .Where(a => (bool)a.Correct)
                        .Select(a => a.Id)
                        .ToArray();

                    return attempt.CorrectAnswers.SequenceEqual(correctAnswers);

                case QuestionType.SingleInput:
                case QuestionType.MultipleInput:

                    if (attempt.CorrectInputs == null) return false;

                    var correctInputs = question.Answers.Select(a => a.Text).ToArray();

                    return attempt.CorrectInputs.SequenceEqual(correctInputs); // normalize
            }

            return false;
        }
    }
}
