using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class Course : ICheckNoteEntity<CourseModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public virtual User Author { get; set; }
        public virtual List<CourseNote> CourseNotes { get; set; }
        public virtual List<CourseLike> Likes { get; set; }
        public virtual List<TestResult> TestResults { get; set; }

        public CourseModel Sanitize() => new CourseModel
        {
            Id = Id,
            Title = Title,
            Description = Description,
            Author = Author
        };

        public static implicit operator CourseModel(Course course) => course.Sanitize();
    }
}
