using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuizPortal.Repositories;
using QuizPortal.Helper;
using System;
using System.Threading.Tasks;

namespace QuizPortal.Controllers
{
    public class ScoresController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public ScoresController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Check if logged in
            var userIdStr = HttpContext.Session.GetString(Constants.SessionUserId);
            if (string.IsNullOrEmpty(userIdStr))
                return RedirectToAction("Login", "User");

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Professor")
                return RedirectToAction("Dashboard", "Student");

            int professorId = Convert.ToInt32(userIdStr);

            var scoreRepo = _repositoryFactory.GetScoreRepository();
            var scores = await scoreRepo.GetScoresByProfessorAsync(professorId);

            return View(scores);
        }
    }
}
