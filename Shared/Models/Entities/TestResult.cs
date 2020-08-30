using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class TestResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Course Course { get; set; }

        [Required]
        public virtual User User { get; set; }

        [Required]
        [Range(0, 100)]
        public int Result { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
