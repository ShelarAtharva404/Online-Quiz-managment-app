using System.Data;
using System.Threading.Tasks;

namespace QuizPortal.Repositories
{
    public interface IRepositoryFactory
    {
        IUserRepository GetUserRepository();

        IQuestionRepository GetQuestionRepository();

        IQuizRepository GetQuizRepository();

        IScoreRepository GetScoreRepository();


        Task<IDbTransaction> BeginTransactionAsync();

        Task SaveAsync();
    }
}
