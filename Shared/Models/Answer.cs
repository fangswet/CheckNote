using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CheckNote.Shared.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public int QuestionId { get; set; }
        public bool? Correct { get; set; }

        public static implicit operator Answer(AnswerModel model) => new Answer
        {
            Text = model.Text,
            Correct = model.Correct
        };
    }

    public class AnswerAttempt
    {
        [Required]
        public int QuestionId { get; set; }
        public bool? Correct { get; set; }
        public int[] CorrectAnswers { get; set; }
        public string[] CorrectInputs { get; set; }
    }
}
