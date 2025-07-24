using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Web.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Class
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var classList = await _context.Classes.Include(c => c.Teacher).ToListAsync();
            // You can set a breakpoint here and inspect classList
            return View(classList);
        }

        // GET: Class/Details/5
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var _class = await _context.Classes
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (_class == null)
            {
                return NotFound();
            }

            var classSubjects = await _context.ClassSubjects
                .Where(cs => cs.ClassId == id)
                .Include(cs => cs.Subject)
                .ToListAsync();
            var allSubjects = await _context.Subjects.ToListAsync();

            var studentIds = await _context.Enrollments
                                .Where(e => e.ClassId == _class.ClassId)
                                .Select(e => e.StudentId)
                                .ToListAsync();
            var students = await _context.Users
                                    .Where(u => studentIds.Contains(u.Id))
                                    .ToListAsync();
            ;
            var viewModel = new ClassSubjectsViewModel
            {
                Class = _class,
                ClassSubjects = classSubjects,
                AllSubjects = allSubjects,
                Students = students
            };

            return View(viewModel);
        }

        // GET: Class/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var data = new ClassTeacherViewModel()
            {
                Teachers =  _context.Users.Where(x => x.Role == "Instructor").ToList()
            };
            return View(data);
        }

        // POST: Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassTeacherViewModel model)
        {
            // Validate TeacherId
            if (string.IsNullOrEmpty(model.TeacherId) || model.TeacherId == "0")
            {
                ModelState.AddModelError("TeacherId", "Please select a valid teacher.");
            }

            if (ModelState.IsValid)
            {
                var newClass = new Class
                {
                    ClassName = model.ClassName,
                    TeacherId = model.TeacherId
                };
                _context.Add(newClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate teacher list for redisplay
            model.Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList();
            return View(model);
        }

        // GET: Class/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            var model = new ClassTeacherViewModel
            {
                ClassId = @class.ClassId,
                ClassName = @class.ClassName,
                TeacherId = @class.TeacherId,
                Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList()
            };
            return View(model);
        }

        // POST: Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassTeacherViewModel model)
        {
            if (id != model.ClassId)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(model.TeacherId) || model.TeacherId == "0")
            {
                ModelState.AddModelError("TeacherId", "Please select a valid teacher.");
            }
            if (ModelState.IsValid)
            {
                var @class = await _context.Classes.FindAsync(id);
                if (@class == null)
                {
                    return NotFound();
                }
                @class.ClassName = model.ClassName;
                @class.TeacherId = model.TeacherId;
                _context.Update(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repopulate teacher list for redisplay
            model.Teachers = _context.Users.Where(x => x.Role == "Instructor").ToList();
            return View(model);
        }

        // GET: Class/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.ClassId == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Class/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassId == id);
        }
    }
}
