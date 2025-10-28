using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPortal.Models
{
    [Table("Quizzes")]
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ArticleId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        // 👇 Add these two new lines:
        [Required]
        public int? CreatedByProfessorId { get; set; }

        [ForeignKey("CreatedByProfessorId")]
        public User? CreatedByProfessor { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
