namespace QuizPortal.Models.Dtos
{
    public class QuizResultDto
    {
        public string Title { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Percentage { get; set; }
    }
}
