using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int NoteId { get; set; }
        public virtual Note Note { get; set; }
        public string Content { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public bool? Correct { get; set; }
        public QuestionDifficulty Difficulty { get; set; }
        public virtual List<Answer> Answers { get; set; }

        public static implicit operator ClosedQuestion(Question question) => new ClosedQuestion
        {
            Id = question.Id,
            Title = question.Title,
            NoteId = question.NoteId,
            Type = question.Type,
            Difficulty = question.Difficulty,
            Correct = (bool)question.Correct
        };

        public static implicit operator QuestionModel(Question question) => new OpenQuestion
        {
            Id = question.Id,
            Title = question.Title,
            NoteId = question.NoteId,
            Type = question.Type,
            Difficulty = question.Difficulty,
            Answers = question.Answers.Select(a => (AnswerModel)a).ToList()
        };
    }

    public abstract class QuestionModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int NoteId { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public QuestionDifficulty Difficulty { get; set; }

        public static implicit operator Question(QuestionModel model) => new Question
        {
            Id = model.Id,
            Title = model.Title,
            NoteId = model.NoteId,
            Type = model.Type,
            Difficulty = model.Difficulty
        };
    }

    public class ClosedQuestion : QuestionModel
    {
        [Required] //check if others inherited
        public bool Correct { get; set; }

        public static implicit operator Question(ClosedQuestion model)
        {
            Question question = model;
            question.Correct = model.Correct;

            return question;
        }
    }

    public class OpenQuestion : QuestionModel
    {
        [Required]
        public List<AnswerModel> Answers { get; set; }

        public static implicit operator Question(OpenQuestion model)
        {
            Question question = model;
            question.Answers = model.Answers.Select(a => (Answer)a).ToList();

            return question;
        }
    }

    public enum QuestionType
    {
        Binary,
        Single,
        Multiple,
        SingleInput,
        MultipleInput
    }

    public enum QuestionDifficulty
    {
        Easy,
        Medium,
        Difficult,
        Bonus
    }
}
