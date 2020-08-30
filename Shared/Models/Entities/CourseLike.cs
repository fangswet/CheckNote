using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class CourseLike
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Course Course { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
