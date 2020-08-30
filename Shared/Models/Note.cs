using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNote.Shared.Models
{
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
        public List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();

        public static implicit operator Note(NoteModel model) => new Note
        {
            Title = model.Title,
            Description = model.Description,
            Content = model.Content,
            ParentId = model.ParentId,
            Sources = model.Sources.Select(s => (Source)s).ToList(),
            Questions = model.Questions.Select(q => (Question)q).ToList()
        };
    }

    public class NoteEntryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public UserModel Author { get; set; }
    }
}
