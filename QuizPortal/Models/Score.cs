using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.Models
{
    [Table("Scores")]
    public class Score
    {
        [Key]
        public int Id { get; set; }

        public int QuizId { get; set; }
        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }

        public int StudentId { get; set; }   // ✅ Use StudentId (not UserId)
        [ForeignKey("StudentId")]
        public User Student { get; set; }

        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
