using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class Course
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

        public List<Question> Questions()
        {
            var questions = new List<Question>();

            CourseNotes.ForEach(cn => questions.AddRange(cn.Note.Questions));

            return questions;
        }

        public int Test(AnswerAttempt[] answers)
        {
            var questions = Questions();

            if (questions.Count == 0) return 0;

            var fullScore = questions.Select(q => (int)q.Difficulty).Sum();
            var score = 0;

            foreach (var answer in answers)
            {
                var question = questions.Find(q => q.Id == answer.QuestionId);
                if (question != null && question.Answer(answer))
                {
                    score += (int)question.Difficulty;
                }
            }

            return (int)(score / (double)fullScore * 100);
        }

        public static implicit operator CourseModel(Course course) => new CourseModel
        {
            Id = course.Id,
            Title = course.Title,
            Description = course.Description,
            Author = course.Author
        };
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
