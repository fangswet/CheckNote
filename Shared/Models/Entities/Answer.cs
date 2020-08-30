using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class Answer : ICheckNoteEntity<AnswerModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public bool? Correct { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [JsonIgnore]
        public virtual Question Question { get; set; }

        public AnswerModel Sanitize() => new AnswerModel
        {
            Id = Id,
            Text = Text,
            QuestionId = QuestionId
        };

        public static implicit operator AnswerModel(Answer answer) => answer.Sanitize();
    }
}
