using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
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
