using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Course : ICheckNoteModel<CourseModel>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public int AuthorId { get; set; }
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

    public class CourseModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public UserModel Author { get; set; }

        public static implicit operator Course(CourseModel model) => new Course
        {
            Title = model.Title,
            Description = model.Description,
        };
    }
}
