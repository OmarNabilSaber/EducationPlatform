using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.ViewModels;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Student")]

    public class StudentExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentExamController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var enrollment = _context.Enrollments.FirstOrDefault( u => u.StudentId == studentId);
            var exams = _context.Exams
                .Include(e => e.Submissions)
                .Where(e => DateTime.Now <= e.AvailableTo && e.ClassId == enrollment.ClassId)
                .Select(e => new ExamListItemViewModel
                {
                    ExamId = e.ExamId,
                    Title = e.Title,
                    Instructions = e.Instructions,
                    AvailableFrom = e.AvailableFrom,
                    AvailableTo = e.AvailableTo,
                    HasSubmitted = e.Submissions.Any(s => s.StudentId == studentId),
                    Score = e.Submissions
                            .Where(s => s.StudentId == studentId)
                            .Select(s => s.Score)
                            .FirstOrDefault()
                }).ToList();

            return View(exams);
        }



        [HttpGet]
        public IActionResult Solve(int id)
        {
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            var alreadySubmitted = _context.ExamSubmissions
                .Any(s => s.ExamId == id && s.StudentId == studentId);

            if (alreadySubmitted)
            {
                TempData["Error"] = "You have already submitted this exam.";
                return RedirectToAction("Index");
            }

            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null) return NotFound();

            var viewModel = new StudentSolveExamViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                TimeLimitMinutes = exam.TimeLimitMinutes,
                Questions = exam.Questions.Select(q => new StudentQuestionAnswerViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Options = q.Options
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Solve(StudentSolveExamViewModel model)
        {
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == model.ExamId);

            if (exam == null) return NotFound();

            int totalScore = 0;
            int score = 0;

            var answers = new Dictionary<int, string>();

            foreach (var question in exam.Questions)
            {
                totalScore += question.Points;

                var studentAnswer = model.Questions
                    .FirstOrDefault(q => q.QuestionId == question.QuestionId)?.Answer;

                answers[question.QuestionId] = studentAnswer;

                if (!string.IsNullOrWhiteSpace(studentAnswer) && studentAnswer == question.CorrectAnswer)
                {
                    score += question.Points;
                }
            }

            // Save submission
            var submission = new ExamSubmission
            {
                ExamId = model.ExamId,
                StudentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!,
                StartedAt = DateTime.UtcNow,
                SubmittedAt = DateTime.UtcNow,
                Score = score,
                Answers = answers
            };

            _context.ExamSubmissions.Add(submission);
            _context.SaveChanges();

            ViewBag.TotalScore = totalScore;
            ViewBag.StudentScore = score;
            TempData["Success"] = $"Your answers were submitted successfully. You scored {score}/{totalScore}.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Result(int id)
        {
            var studentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var student = _context.Users.FirstOrDefault(s => s.Id == studentId);
            var submission = _context.ExamSubmissions
                .Include(s => s.Exam)
                    .ThenInclude(e => e.Questions)
                .FirstOrDefault(s => s.ExamId == id && s.StudentId == studentId);

            if (submission == null)
                return NotFound();

            var viewModel = new StudentSolveExamViewModel
            {
                ExamId = submission.ExamId,
                Title = submission.Exam.Title,
                PassingScore = submission.Exam.PassingScore,
                Questions = submission.Exam.Questions.Select(q => new StudentQuestionAnswerViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Options = q.Options ?? new List<string>(),
                    Answer = submission.Answers != null && submission.Answers.ContainsKey(q.QuestionId)
                ? submission.Answers[q.QuestionId]
                : null,
                    CorrectAnswer = q.CorrectAnswer
                }).ToList()

            };

            ViewBag.StudentScore = submission.Score;
            return View("Result", viewModel);
        }


    }
}
