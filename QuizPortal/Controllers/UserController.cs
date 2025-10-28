using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.Helper;
using QuizPortal.Models;
using QuizPortal.Models.Dtos;
using QuizPortal.Repositories;

namespace QuizPortal.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public UserController(IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        // ===============================
        // REGISTER (PROFESSOR)
        // ===============================
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "User register validation failed" });

            IUserRepository userRepository = _repositoryFactory.GetUserRepository();

            if (await userRepository.UserExistsAsync(userDto.Username))
                return Json(new { success = false, message = $"User already exists: {userDto.Username}" });

            User user = _mapper.Map<User>(userDto);
            user.Role = "Professor";

            await userRepository.CreateUserAsync(user);
            await _repositoryFactory.SaveAsync();

            return Json(new { success = true, message = "Registration successful!", url = Url.Action("Login", "User") });
        }

        // ===============================
        // LOGIN (PROFESSOR OR STUDENT)
        // ===============================
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "User login validation failed" });

            IUserRepository userRepository = _repositoryFactory.GetUserRepository();

            if (!await userRepository.UserExistsAsync(userDto.Username, userDto.Password))
                return Json(new { success = false, message = "Username or password is incorrect" });

            var userFromDb = await userRepository.GetUserAsync(userDto.Username);

            if (string.IsNullOrEmpty(userFromDb.Role))
            {
                userFromDb.Role = "Professor";
                await _repositoryFactory.SaveAsync();
            }

            if (!string.Equals(userFromDb.Role, userDto.Role, System.StringComparison.OrdinalIgnoreCase))
            {
                return Json(new
                {
                    success = false,
                    message = $"Invalid role selected for {userDto.Username}. Please choose {userFromDb.Role}."
                });
            }

            // ✅ Session
            HttpContext.Session.SetString(Constants.SessionUserId, userFromDb.Id.ToString());
            HttpContext.Session.SetString("UserRole", userFromDb.Role);

            string redirectUrl = userFromDb.Role == "Professor"
                ? Url.Action("Index", "Home")
                : Url.Action("Dashboard", "Student");

            return Json(new { success = true, message = "Login successful", url = redirectUrl });
        }

        // ===============================
        // LOGOUT
        // ===============================
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        // ===============================
        // ADD STUDENT (PROFESSOR ONLY)
        // ===============================
        [HttpGet]
        public IActionResult AddStudent()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Professor")
                return RedirectToAction("Login", "User");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(UserDto studentDto)
        {
            var role = HttpContext.Session.GetString("UserRole");
            var professorIdStr = HttpContext.Session.GetString(Constants.SessionUserId);

            if (role != "Professor" || professorIdStr == null)
                return Json(new { success = false, message = "Access denied. Only professors can add students." });

            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid student details." });

            IUserRepository userRepository = _repositoryFactory.GetUserRepository();

            if (await userRepository.UserExistsAsync(studentDto.Username))
                return Json(new { success = false, message = "Username already exists." });

            var student = _mapper.Map<User>(studentDto);
            student.Role = "Student";
            student.CreatedByProfessorId = int.Parse(professorIdStr);

            await userRepository.CreateUserAsync(student);
            await _repositoryFactory.SaveAsync();

            return Json(new { success = true, message = "Student added successfully!" });
        }

        // ===============================
        // MANAGE STUDENTS (CRUD VIEW)
        // ===============================
        [HttpGet]
        // 📋 Manage Students (for Professors)
[HttpGet]
public async Task<IActionResult> ManageStudents()
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
