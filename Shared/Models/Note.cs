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
        public virtual List<CourseNote> Courses { get; set; }
        public virtual List<Note> Children { get; set; }
        public virtual List<Question> Questions { get; set; }

        public static implicit operator NoteModel(Note note) => new NoteModel
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            Content = note.Content,
            ParentId = note.ParentId,
            Children = note.Children.Select(child => (NoteModel)child).ToList(),
            Author = note.Author
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

        public static implicit operator Note(NoteModel model) => new Note
        {
            Title = model.Title,
            Description = model.Description,
            Content = model.Content,
            ParentId = model.ParentId
        };
    }
}
