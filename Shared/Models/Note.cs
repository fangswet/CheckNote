using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public virtual Note Parent { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
        public virtual List<CourseNote> CourseNotes { get; set; }
        public virtual List<Note> Children { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Source> Sources { get; set; }

        public int Test(AnswerAttempt[] answers)
        {
            if (Questions.Count == 0) return 0;

            var fullScore = Questions.Select(q => (int)q.Difficulty).Sum();
            int score = 0;

            foreach (var answer in answers)
            {
                var question = Questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question != null && question.Answer(answer))
                {
                    score += (int)question.Difficulty;
                }
            }

            return (int)(score / (double)fullScore * 100);
        }

        public static implicit operator NoteModel(Note note) => new NoteModel
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            Content = note.Content,
            ParentId = note.ParentId,
            Children = note.Children.Select(child => (NoteModel)child).ToList(),
            Author = note.Author,
            Sources = note.Sources.Select(s => (SourceModel)s).ToList()
        };
    }

    public class NoteModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        public int? ParentId { get; set; }
        public List<NoteModel> Children { get; set; }
        public UserModel Author { get; set; }
        public List<SourceModel> Sources { get; set; } = new List<SourceModel>();

        public static implicit operator Note(NoteModel model) => new Note
        {
            Title = model.Title,
            Description = model.Description,
            Content = model.Content,
            ParentId = model.ParentId,
            Sources = model.Sources.Select(s => (Source)s).ToList()
        };
    }
}
