﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using System.Security.AccessControl;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize(Roles = "Admin, Instructor")]
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Index()
        {
            var exams = _context.Exams.Include(s => s.Subject).Include(e => e.Class).ToList();

            return View(exams);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create()
        {
            var classes = _context.Classes.ToList();
            var subjects = _context.Subjects.ToList();

            //if (!classes.Any())
            //{
            //    return NotFound("No classes found.");
            //}

            var examViewModel = new ExamViewModel
            {
                Classes = classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList(),

                Subjects = subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(examViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create(ExamViewModel exam)
        {
            if (!ModelState.IsValid)
            {
                exam.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList();
                if (exam.AvailableFrom >= exam.AvailableTo)
                {
                    ModelState.AddModelError("", "Available From must be earlier than Available To.");
                }

                exam.Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                }).ToList();
                return View(exam);
            }

            var newExam = new Exam
            {
                Title = exam.Title,
                Instructions = exam.Instructions,
                AvailableFrom = exam.AvailableFrom,
                AvailableTo = exam.AvailableTo,
                TimeLimitMinutes = exam.TimeLimitMinutes,
                PassingScore = exam.PassingScore,
                ClassId = exam.ClassId,
                SubjectId = exam.SubjectId,
                Questions = exam.Questions?.Select(q => new Question
                {
                    CorrectAnswer = q.CorrectAnswer,
                    Options = q.Options,
                    Points = q.Points,
                    Text = q.Text
                }).ToList() ?? new List<Question>()
            };

            _context.Exams.Add(newExam);
            _context.SaveChanges();
            SendNotificatons(exam.ClassId, "New exam has been added recentlly check it out");
            _context.SaveChanges();
            TempData["Success"] = "Exam created successfully!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Details(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Class)
                .Include(s => s.Subject)
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Edit(int id)
        {
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == id);

            if (exam == null) return NotFound();

            var viewModel = new ExamViewModel
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Instructions = exam.Instructions,
                AvailableFrom = exam.AvailableFrom,
                AvailableTo = exam.AvailableTo,
                TimeLimitMinutes = exam.TimeLimitMinutes,
                PassingScore = exam.PassingScore,
                ClassId = exam.ClassId,
                SubjectId = exam.SubjectId,
                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text,
                    Points = q.Points,
                    CorrectAnswer = q.CorrectAnswer,
                    Options = q.Options
                }).ToList(),
                Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList(),
                Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Edit(ExamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassId.ToString(),
                    Text = c.ClassName
                }).ToList();
                model.Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.Name
                }).ToList();

                return View(model);
            }
            if (model.AvailableFrom >= model.AvailableTo)
            {
                ModelState.AddModelError("", "Available From must be earlier than Available To.");
            }
            var exam = _context.Exams
                .Include(e => e.Questions)
                .FirstOrDefault(e => e.ExamId == model.ExamId);

            if (exam == null) return NotFound();

            // Update exam fields
            exam.Title = model.Title;
            exam.Instructions = model.Instructions;
            exam.AvailableFrom = model.AvailableFrom;
            exam.AvailableTo = model.AvailableTo;
            exam.TimeLimitMinutes = model.TimeLimitMinutes;
            exam.PassingScore = model.PassingScore;
            exam.ClassId = model.ClassId;

            // Remove deleted questions
            var incomingIds = model.Questions.Where(q => q.QuestionId != 0).Select(q => q.QuestionId).ToList();
            var toRemove = exam.Questions.Where(q => !incomingIds.Contains(q.QuestionId)).ToList();
            _context.Questions.RemoveRange(toRemove);

            // Update or add questions
            foreach (var q in model.Questions)
            {
                if (q.QuestionId != 0)
                {
                    // Update existing
                    var existing = exam.Questions.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                    if (existing != null)
                    {
                        existing.Text = q.Text;
                        existing.Points = q.Points;
                        existing.CorrectAnswer = q.CorrectAnswer;
                        existing.Options = q.Options;
                    }
                }
                else
                {
                    // Add new
                    exam.Questions.Add(new Question
                    {
                        Text = q.Text,
                        Points = q.Points,
                        CorrectAnswer = q.CorrectAnswer,
                        Options = q.Options,
                        ExamId = exam.ExamId // Ensure new questions are linked to the exam
                    });
                }
            }

            _context.SaveChanges();
            SendNotificatons(exam.ClassId, $"{exam.Title} exam has been updated recentlly check it out");
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Delete(int id)
        {
            var exam = _context.Exams.Include(e => e.Questions).FirstOrDefault(e => e.ExamId == id);
            if (exam == null)
            {
                return NotFound();
            }
            _context.Questions.RemoveRange(exam.Questions);
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private void SendNotificatons(int classId, string message = "new exam notification")
        {
            var _class = _context.Classes.FirstOrDefault(Class => Class.ClassId == classId);
            var studentsId = _context.Enrollments.Where(c => c.ClassId == classId).Select(s => s.StudentId);
            if (_class != null && _class.TeacherId != null)
            { 
                Notification.SendNotification(_context, _class.TeacherId, message, "Exam Notification");
            }
            foreach (var studentId in studentsId) 
            {
                Notification.SendNotification(_context, studentId, message, "Exam Notification");
            }
        }
        public async Task<IActionResult> Submitions(int id)
        {
            var Exam = await _context.Exams
                .Include(e => e.Class)
                    .ThenInclude(c => c.Enrollments)
                        .ThenInclude(e => e.Student)
                .FirstOrDefaultAsync(e => e.ExamId == id);

            if (Exam == null)
                return NotFound();

            var studentsId = _context.Enrollments.Where(c => c.ClassId == Exam.ClassId).Select(s => s.StudentId);
            var students = _context.Users.Where(u => studentsId.Contains(u.Id));
            var ExamSubmitions = _context.ExamSubmissions.Where(u => studentsId.Contains(u.StudentId));

            var studentGrades = students.Select(s => new StudentGradeViewModel
            {
                StudintId = s.Id,
                StudentName = s.FullName,
                StudentEmail = s.Email,
                IsSubmitted = ExamSubmitions.Any(es => es.StudentId == s.Id),
                Score = ExamSubmitions
                            .Any(es => es.StudentId == s.Id)
                            ? ExamSubmitions
                                .FirstOrDefault(es => es.StudentId == s.Id).Score
                            : (int?)null
            }).ToList();

            var assignmentGrades = new ExamGradeViewModel
            {
                ExamId = Exam.ExamId,
                ExamName = Exam.Title,
                PassingScore = Exam.PassingScore,
                ClassId = Exam.ClassId,
                ClassName = Exam.Class.ClassName,
                AvailableFrom = Exam.AvailableFrom,
                AvailableTo = Exam.AvailableTo,
                StudentGrades = studentGrades
            };

            return View(assignmentGrades);
        }
        public IActionResult Result(int examId, string studentId)
        {
            var student = _context.Users.FirstOrDefault(s => s.Id == studentId);
            var submission = _context.ExamSubmissions
                .Include(s => s.Exam)
                    .ThenInclude(e => e.Questions)
                .FirstOrDefault(s => s.ExamId == examId && s.StudentId == studentId);

            if (submission == null)
                return NotFound();

            var viewModel = new StudentSolveExamViewModel
            {
                ExamId = submission.ExamId,
                Title = submission.Exam.Title,
                PassingScore = submission.Exam.PassingScore,
                StudentName = student.FullName,
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
