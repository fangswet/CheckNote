using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Note : ICheckNoteModel<NoteModel>
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

        public NoteModel Sanitize() => new NoteModel
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Content = Content,
            ParentId = ParentId,
            Children = Children.Select(child => (NoteModel)child).ToList(),
            Author = Author,
            Sources = Sources.Select(s => (SourceModel)s).ToList()
        };

        public static implicit operator NoteModel(Note note) => note.Sanitize();
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
