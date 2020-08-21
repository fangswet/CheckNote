using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class Source
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        public string Comment { get; set; }
        [Required]
        public SourceType Type { get; set; }
        [Required]
        public virtual int NoteId { get; set; }
        public virtual Note Note { get; set; }

        public static implicit operator SourceModel(Source source) => new SourceModel
        {
            Text = source.Text,
            Comment = source.Comment,
            Type = source.Type
        };
    }

    public class SourceModel
    {
        [Required]
        public string Text { get; set; }
        public string Comment { get; set; }
        [Required]
        public SourceType Type { get; set; }

        public static implicit operator Source(SourceModel model) => new Source
        {
            Text = model.Text,
            Comment = model.Comment,
            Type = model.Type
        };
    }

    public enum SourceType
    {
        Book,
        Article,
        Video
    }
}
