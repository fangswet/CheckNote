using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
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
