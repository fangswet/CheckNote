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
        public Note Note { get; set; }
    }

    public enum SourceType
    {
        Book,
        Article,
        Video
    }
}
