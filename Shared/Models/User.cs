using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CheckNote.Shared.Models
{
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
