using Microsoft.EntityFrameworkCore;
using QuizPortal.Data;
using QuizPortal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizPortal.Repositories.Sqlite
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly AppDbContext _db;

        public ScoreRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task CreateScoreAsync(Score score)
        {
            await _db.Scores.AddAsync(score);
        }

        public async Task<ICollection<Score>> GetAllScoresAsync()
        {
            return await _db.Scores
                .Include(s => s.Student)
                .Include(s => s.Quiz)
                .ToListAsync();
        }

        public async Task<ICollection<Score>> GetScoresByQuizIdAsync(int quizId)
        {
            return await _db.Scores
                .Where(s => s.QuizId == quizId)
                .Include(s => s.Student)
                .ToListAsync();
        }

        public async Task<ICollection<Score>> GetScoresByStudentIdAsync(int studentId)
        {
            return await _db.Scores
                .Where(s => s.StudentId == studentId)
                .Include(s => s.Quiz)
                .ToListAsync();
        }

        // ✅ Add this for professor dashboard
        public async Task<ICollection<Score>> GetScoresByProfessorAsync(int professorId)
        {
            return await _db.Scores
                .Include(s => s.Student)
                .Include(s => s.Quiz)
                .Where(s => s.Quiz.CreatedByProfessorId == professorId)
                .ToListAsync();
        }
    }
}
