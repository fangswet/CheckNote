using CheckNote.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services.Extensions
{
    public static class CourseExtensions
    {
        public static List<Note> GetNotes(this Course course)
            => course.CourseNotes.Select(cn => cn.Note).ToList();

        public static List<Question> GetQuestions(this Course course)
        {
            var questions = new List<Question>();

            course.CourseNotes.ForEach(cn => questions.AddRange(cn.Note.Questions));

            return questions;
        }

        public static int Practice(this Course course, AnswerAttempt[] answers)
        {
            var questions = course.GetQuestions();

            if (questions.Count == 0) return 0;

            var fullScore = questions.Select(q => (int)q.Difficulty).Sum();
            var score = 0;

            foreach (var answer in answers)
            {
                var question = questions.Find(q => q.Id == answer.QuestionId);
                if (question != null && question.Answer(answer))
                {
                    score += (int)question.Difficulty;
                }
            }

            return (int)(score / (double)fullScore * 100);
        }
    }
}
