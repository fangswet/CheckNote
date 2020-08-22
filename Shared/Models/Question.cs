using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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

        public static implicit operator QuestionModel(Question question) => new QuestionModel
        {
            Id = question.Id,
            Title = question.Title,
            NoteId = question.NoteId,
            Content = question.Content,
            Type = question.Type,
            Correct = question.Correct,
            Difficulty = question.Difficulty,
            Answers = question.Answers.Select(a => (AnswerModel)a).ToList()
        };
    }

    public class QuestionModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        //[Required] // write methods in models
        public int NoteId { get; set; }
        public string Content { get; set; }
        public QuestionType Type { get; set; }
        public bool? Correct { get; set; }
        public QuestionDifficulty Difficulty { get; set; } = QuestionDifficulty.Easy;
        public virtual List<AnswerModel> Answers { get; set; }

        public static implicit operator Question(QuestionModel model) => new Question
        {
            Title = model.Title,
            NoteId = model.NoteId,
            Content = model.Content,
            Type = model.Type,
            Correct = model.Correct,
            Difficulty = model.Difficulty,
            Answers = model.Answers?.Select(a => (Answer)a).ToList()
        };
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
        Easy = 1,
        Medium,
        Difficult,
        Bonus
    }
}
