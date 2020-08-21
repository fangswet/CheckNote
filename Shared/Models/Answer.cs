using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CheckNote.Shared.Models
{
    public class Answer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public bool? Correct { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public static implicit operator AnswerModel(Answer answer) => new AnswerModel
        {
            Id = answer.Id,
            Text = answer.Text,
            QuestionId = answer.QuestionId
        };
    }

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
        public int QuestionId { get; set; }
        public bool? Correct { get; set; }
        public int[] CorrectAnswers { get; set; }
        public string[] CorrectInputs { get; set; }
    }
}
