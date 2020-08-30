using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Note : ICheckNoteEntity<NoteModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        [JsonIgnore]
        public virtual Note Parent { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public virtual User Author { get; set; }
        public virtual List<CourseNote> CourseNotes { get; set; }
        public virtual List<Note> Children { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Source> Sources { get; set; }

        public NoteModel Sanitize() => new NoteModel
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Content = Content,
            ParentId = ParentId,
            Children = Children.Select(c => (NoteModel)c).ToList(),
            Author = Author,
            Sources = Sources.Select(s => (SourceModel)s).ToList(),
            Questions = Questions.Select(q => (QuestionModel)q).ToList()
        };

        public static implicit operator NoteModel(Note note) => note.Sanitize();
        public static implicit operator NoteEntryModel(Note note) => new NoteEntryModel
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            ParentId = note.ParentId,
            Author = note.Author
        };
    }
}
