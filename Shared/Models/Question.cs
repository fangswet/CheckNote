using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class Question
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        public bool? Correct { get; set; }

        public QuestionDifficulty Difficulty { get; set; }

        public List<Answer> Answers { get; }
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
