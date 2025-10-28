using System.Collections.Generic;
using System.Threading.Tasks;
using QuizPortal.Models;

namespace QuizPortal.Repositories
{
    public interface IScoreRepository
    {
        Task CreateScoreAsync(Score score);
        Task<ICollection<Score>> GetAllScoresAsync();
        Task<ICollection<Score>> GetScoresByQuizIdAsync(int quizId);
        Task<ICollection<Score>> GetScoresByStudentIdAsync(int studentId);

        // ✅ Add this new method for professor dashboard
        Task<ICollection<Score>> GetScoresByProfessorAsync(int professorId);
    }
}
