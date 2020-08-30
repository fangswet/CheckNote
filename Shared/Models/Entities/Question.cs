using Microsoft.EntityFrameworkCore.Storage.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Question : ICheckNoteEntity<QuestionModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int NoteId { get; set; }
        [JsonIgnore]
        public virtual Note Note { get; set; }
        public string Content { get; set; }
        [Required]
        public QuestionType Type { get; set; }
        public bool? Correct { get; set; }
        public QuestionDifficulty Difficulty { get; set; }
        public virtual List<Answer> Answers { get; set; }

        public QuestionModel Sanitize() => new QuestionModel
        {
            Id = Id,
            Title = Title,
            NoteId = NoteId,
            Content = Content,
            Type = Type,
            Difficulty = Difficulty,
            Answers = Answers.Select(a => (AnswerModel)a).ToList()
        };

        public static implicit operator QuestionModel(Question question) => question.Sanitize();
    }
}
