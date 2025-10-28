using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuizPortal.Repositories;
using QuizPortal.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using QuizPortal.Helper;

namespace QuizPortal.Controllers
{
    public class StudentController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public StudentController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        // 🧑‍🎓 Student Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Student")
                return RedirectToAction("Login", "User");

            return View();
        }

        // 📋 Manage Students (for Professors)
        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Professor")
                return RedirectToAction("Login", "User");

            var userRepo = _repositoryFactory.GetUserRepository();
            var professorId = Convert.ToInt32(HttpContext.Session.GetString(Constants.SessionUserId));

            var students = await userRepo.GetAllAsync();
            var studentList = students.Where(u => u.Role == "Student" && u.CreatedByProfessorId == professorId).ToList();

            return View(studentList);
        }

        // ✏️ Edit Student
        [HttpPost]
        public async Task<IActionResult> EditStudent(int id, string username, string password)
        {
            var repo = _repositoryFactory.GetUserRepository();
            var user = await repo.GetByIdAsync(id);

            if (user == null || user.Role != "Student")
                return Json(new { success = false, message = "Student not found" });

            user.Username = username;
            user.Password = password;
            user.Updated = DateTime.UtcNow;

            await _repositoryFactory.SaveAsync();
            return Json(new { success = true, message = "Student updated successfully" });
        }

        // ❌ Delete Student
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var repo = _repositoryFactory.GetUserRepository();
            var user = await repo.GetByIdAsync(id);

            if (user == null || user.Role != "Student")
                return Json(new { success = false, message = "Student not found" });

            repo.Delete(user);
            await _repositoryFactory.SaveAsync();

            return Json(new { success = true, message = "Student deleted successfully" });
        }
    }
}
