using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class User : IdentityUser<int>, ICheckNoteModel<UserModel>
    {
        public virtual List<Note> Notes { get; set; }
        public virtual List<CourseLike> Likes { get; set; }
        public virtual List<TestResult> TestResults { get; set; }

        public static implicit operator UserModel(User user) => new UserModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        public static implicit operator UserNotesModel(User user)
        {
            UserNotesModel notesModel = user;

            notesModel.Notes = user.Notes.Select(n => (NoteModel)n).ToList();

            return notesModel;
        }
    }

    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        
        public static implicit operator User(UserModel model) => new User
        {
            Id = model.Id,
            UserName = model.UserName,
            Email = model.Email
        };
    }

    public class UserNotesModel : UserModel
    {
        public List<NoteModel> Notes { get; set; }
    }
}
