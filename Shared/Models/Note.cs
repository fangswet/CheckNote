using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace CheckNote.Shared.Models
{
    public class Note
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Note Parent { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public User Author { get; set; }

        public List<CourseNote> Courses { get; set; } = default;

        public List<Note> Children { get; set; } = default;

        public static implicit operator NoteOutput(Note note)
        {
            var output = new NoteOutput
            {
                Title = note.Title,
                Description = note.Description,
                Content = note.Content,
                Children = note.Children.Select(child => (NoteOutput)child).ToList(),
                Author = note.Author
            };

            if (note.Parent != null) output.Parent = note.Parent.Id;

            return output;
        }
    }

    public class NoteInput
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        public int? Parent { get; set; }

        public static implicit operator Note(NoteInput input)
        {
            var note = new Note
            {
                Title = input.Title,
                Description = input.Description,
                Content = input.Content
            };

            if (input.Parent != null) note.Parent = new Note
            {
                Id = (int) input.Parent
            };

            return note;
        }
    }

    public class NoteOutput
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public int Parent { get; set; }

        public UserOutput Author { get; set; }

        public List<NoteOutput> Children { get; set; }
    }
}
