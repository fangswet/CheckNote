﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    // remove when convention many-to-many supported
    public class CourseNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Course Course { get; set; }

        [Required]
        public Note Note { get; set; }
    }
}