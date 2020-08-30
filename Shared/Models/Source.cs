using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
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
