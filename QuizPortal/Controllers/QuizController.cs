using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizPortal.Helper;
using QuizPortal.Models;
using QuizPortal.Models.Dtos;
using QuizPortal.Proxies;
using QuizPortal.Repositories;

namespace QuizPortal.Controllers
{
    public class QuizController : Controller
    {
        private readonly IWiredProxy _wiredProxy;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IMapper _mapper;

        public QuizController(IWiredProxy wiredProxy, IRepositoryFactory repositoryFactory, IMapper mapper)
        {
            _wiredProxy = wiredProxy;
            _repositoryFactory = repositoryFactory;
            _mapper = mapper;
        }

        [BindProperty]
        public CreateQuizViewDto CreateQuizViewDto { get; set; }

        // =======================================================
        // GET: Create Quiz (Only for Professor)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> CreateQuiz()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
                return RedirectToAction("Login", "User");

            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Professor")
                return RedirectToAction("Dashboard", "Student");

            var articleList = await _wiredProxy.GetLastFiveArticlesAsync();

            CreateQuizViewDto = new CreateQuizViewDto
            {
                ArticleList = articleList.ToList(),
                QuestionArr = new QuestionDto[10]
            };

            return View(CreateQuizViewDto);
        }

        // =======================================================
        // POST: Create Quiz
        // =======================================================
        [HttpPost]
        [ActionName("CreateQuiz")]
        public async Task<IActionResult> CreateQuizPost()
        {
            CreateQuizViewDto.ErrorMessage = null;

            if (!ModelState.IsValid)
            {
                CreateQuizViewDto.ErrorMessage = "Invalid quiz details.";
                return View(CreateQuizViewDto);
            }

            var validQuestions = CreateQuizViewDto.QuestionArr
                .Where(q => !string.IsNullOrWhiteSpace(q.QuestionText))
                .ToList();

            if (validQuestions.Count < 1)
            {
                CreateQuizViewDto.ErrorMessage = "Please add at least one question.";
                return View(CreateQuizViewDto);
            }

            if (validQuestions.Select(q => q.QuestionText.Trim()).Distinct().Count() != validQuestions.Count)
            {
                CreateQuizViewDto.ErrorMessage = "Each question must be unique.";
                return View(CreateQuizViewDto);
            }

            foreach (var q in validQuestions)
            {
                var answers = new List<string> { q.AnswerA, q.AnswerB, q.AnswerC, q.AnswerD };
                if (answers.Distinct().Count() != answers.Count)
                {
                    CreateQuizViewDto.ErrorMessage = $"Question \"{q.QuestionText}\" has duplicate answers.";
                    return View(CreateQuizViewDto);
                }
            }

            var transaction = await _repositoryFactory.BeginTransactionAsync();
            try
            {
                var quizRepository = _repositoryFactory.GetQuizRepository();

                var selectedArticle = CreateQuizViewDto.ArticleList
                    .FirstOrDefault(a => a.ArticleId == CreateQuizViewDto.SelectedArticleId);

                if (selectedArticle == null)
                {
                    CreateQuizViewDto.ErrorMessage = "Please select a valid article.";
                    transaction.Rollback();
                    return View(CreateQuizViewDto);
                }

                var quiz = _mapper.Map<Quiz>(selectedArticle);

                var professorIdStr = HttpContext.Session.GetString(Constants.SessionUserId);
                if (int.TryParse(professorIdStr, out int professorId))
                    quiz.CreatedByProfessorId = professorId;

                await quizRepository.CreateQuizAsync(quiz);
                await _repositoryFactory.SaveAsync();

                var questionRepository = _repositoryFactory.GetQuestionRepository();
                foreach (var item in validQuestions)
                {
                    var question = _mapper.Map<Question>(item);
                    question.QuizId = quiz.Id;
                    await questionRepository.CreateQuestionAsync(question);
                }

                await _repositoryFactory.SaveAsync();
                transaction.Commit();

                return RedirectToAction("Index", "Quiz");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                CreateQuizViewDto.ErrorMessage = $"Error creating quiz: {ex.Message}";
                return View(CreateQuizViewDto);
            }
        }

        // =======================================================
        // GET: Quiz List (Professor or Student)
        // =======================================================
        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
                return RedirectToAction("Login", "User");

            return View();
        }

        // =======================================================
        // AJAX: Get All Quizzes (DataTables)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> GetAllQuizzes()
        {
            try
            {
                var userIdStr = HttpContext.Session.GetString(Constants.SessionUserId);
                var role = HttpContext.Session.GetString("UserRole");

                if (string.IsNullOrEmpty(userIdStr))
                    return Json(new { data = new List<object>(), error = "Session expired" });

                var quizRepository = _repositoryFactory.GetQuizRepository();
                var quizList = await quizRepository.GetAllQuizzesAsync();

                // Filter only professor’s quizzes if Professor
                if (role == "Professor" && int.TryParse(userIdStr, out int professorId))
                    quizList = quizList.Where(q => q.CreatedByProfessorId == professorId).ToList();

                var quizDtoList = _mapper.Map<List<QuizDto>>(quizList);

                return Json(new { data = quizDtoList });
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<object>(), error = ex.Message });
            }
        }

        // =======================================================
        // DELETE: Delete Quiz
        // =======================================================
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var quizRepository = _repositoryFactory.GetQuizRepository();
                var quizFromDb = await quizRepository.GetQuizAsync(id);

                if (quizFromDb == null)
                    return Json(new { success = false, message = "Quiz not found." });

                quizRepository.DeleteQuiz(quizFromDb);
                await _repositoryFactory.SaveAsync();

                return Json(new { success = true, message = "Quiz deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // =======================================================
        // GET: Take Quiz (Student)
        // =======================================================
        [HttpGet]
        public async Task<IActionResult> Quiz(int id)
        {
            if (HttpContext.Session.GetString(Constants.SessionUserId) == null)
                return RedirectToAction("Login", "User");

            var quizRepository = _repositoryFactory.GetQuizRepository();
            var questionRepository = _repositoryFactory.GetQuestionRepository();

            var quizFromDb = await quizRepository.GetQuizAsync(id);
            if (quizFromDb == null)
                return RedirectToAction("Index", "Quiz");

            var questionList = await questionRepository.GetAllQuestionsAsync(id);
            var quizDto = _mapper.Map<QuizDto>(quizFromDb);
            var questionDtoList = _mapper.Map<List<QuestionDto>>(questionList);

            var quizViewDto = new QuizViewDto
            {
                QuizDto = quizDto,
                QuestionDtoList = questionDtoList
            };

            return View(quizViewDto);
        }

        // =======================================================
        // POST: Submit Quiz (with score)
        // =======================================================
        [HttpPost]
public async Task<IActionResult> SubmitQuiz(QuizViewDto quizView)
{
    if (quizView == null || quizView.QuestionDtoList == null)
        return BadRequest("Invalid quiz data.");

    int total = quizView.QuestionDtoList.Count;
    int correct = 0;

    foreach (var q in quizView.QuestionDtoList)
    {
        if (!string.IsNullOrEmpty(q.SelectedOption) &&
            string.Equals(q.SelectedOption.Trim(), q.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            correct++;
        }
    }

    double percentage = (double)correct / total * 100;

    // ✅ Get Student ID from Session
    var userIdStr = HttpContext.Session.GetString(Constants.SessionUserId);
    if (!int.TryParse(userIdStr, out int studentId))
        return RedirectToAction("Login", "User");

    var scoreRepository = _repositoryFactory.GetScoreRepository();

    var score = new Score
    {
        StudentId = studentId,               // ✅ FIXED
        QuizId = quizView.QuizDto.Id,
        CorrectAnswers = correct,
        TotalQuestions = total,
        Percentage = Math.Round(percentage, 2),
        SubmittedAt = DateTime.UtcNow        // ✅ FIXED
    };

    await scoreRepository.CreateScoreAsync(score);
    await _repositoryFactory.SaveAsync();

    var resultDto = new QuizResultDto
    {
        Title = quizView.QuizDto.Title,
        TotalQuestions = total,
        CorrectAnswers = correct,
        Percentage = Math.Round(percentage, 2)
    };

    return View("QuizResult", resultDto);
}


    }
}
