using QuizPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizPortal.Repositories
{
    public interface IQuizRepository
    {
        Task CreateQuizAsync(Quiz quiz);
        Task<Quiz> GetQuizAsync(int id);
        Task<ICollection<Quiz>> GetAllQuizzesAsync();
        Task<ICollection<Quiz>> GetQuizzesByProfessorAsync(int professorId); // 👈 Add this
        void DeleteQuiz(Quiz quiz);
    }
}
