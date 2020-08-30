using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckNote.Shared.Models
{
    public class User : IdentityUser<int>, ICheckNoteEntity<UserModel>
    {
        public virtual List<Note> Notes { get; set; }
        public virtual List<CourseLike> Likes { get; set; }
        public virtual List<TestResult> TestResults { get; set; }

        public UserModel Sanitize() => new UserModel
        {
            Id = Id,
            UserName = UserName,
            Email = Email
        };

        public static implicit operator UserModel(User user) => user.Sanitize();

        public static implicit operator UserNotesModel(User user)
        {
            UserNotesModel notesModel = user;

            notesModel.Notes = user.Notes.Select(n => (NoteModel)n).ToList();

            return notesModel;
        }
    }
}
