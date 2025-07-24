using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineEducationPlatform.Infrastructure.Data;
using OnlineEducationPlatform.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Web.Controllers
{
    
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Book
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        // GET: Book/Details/5
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Create([Bind("BookId,Name,Description")] Book book, IFormFile imageFile, IFormFile bookFile)
        {
            ModelState.Remove("ImageUrl");
            ModelState.Remove("BookUrl");
            // Validate and save image file
            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var imageExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedImageExtensions.Contains(imageExt))
                {
                    ModelState.AddModelError("ImageUrl", "Only JPG, JPEG, and PNG files are allowed for the image.");
                    return View(book);
                }
            }
            else
            {
                ModelState.AddModelError("ImageUrl", "Book image is required.");
                return View(book);
            }

            // Validate and book file
            if (bookFile != null && bookFile.Length > 0)
            {
                var bookExt = Path.GetExtension(bookFile.FileName).ToLowerInvariant();
                if (bookExt != ".pdf")
                {
                    ModelState.AddModelError("BookUrl", "Only PDF files are allowed for the book.");
                    return View(book);
                }
                
            }
            else
            {
                ModelState.AddModelError("BookUrl", "Book PDF is required.");
                return View(book);
            }


            if (ModelState.IsValid)
            {
                // save image file
                var imageExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                var imageFileName = $"bookimg_{Guid.NewGuid()}{imageExt}";
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Books", imageFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                book.ImageUrl = $"/Books/{imageFileName}";

                // save book file
                var bookExt = Path.GetExtension(bookFile.FileName).ToLowerInvariant();
                var bookFileName = $"bookfile_{Guid.NewGuid()}{bookExt}";
                var bookPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Books", bookFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(bookPath));
                using (var stream = new FileStream(bookPath, FileMode.Create))
                {
                    await bookFile.CopyToAsync(stream);
                }
                book.BookUrl = $"/Books/{bookFileName}";


                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Name,Description,ImageUrl,BookUrl")] Book book, IFormFile imageFile, IFormFile bookFile)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            // Remove validation for ImageUrl and BookUrl so we can handle them manually
            ModelState.Remove("imageFile");
            ModelState.Remove("bookFile");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("BookUrl");

            // Get the original book from the database
            var originalBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.BookId == id);
            if (originalBook == null)
            {
                return NotFound();
            }

            // Handle image file upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var imageExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedImageExtensions.Contains(imageExt))
                {
                    ModelState.AddModelError("ImageUrl", "Only JPG, JPEG, and PNG files are allowed for the image.");
                    return View(book);
                }
                var imageFileName = $"bookimg_{Guid.NewGuid()}{imageExt}";
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","Books", imageFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                book.ImageUrl = $"/Books/{imageFileName}";
            }
            else
            {
                // Keep the original image if no new file is uploaded
                book.ImageUrl = originalBook.ImageUrl;
            }

            // Handle book file upload
            if (bookFile != null && bookFile.Length > 0)
            {
                var bookExt = Path.GetExtension(bookFile.FileName).ToLowerInvariant();
                if (bookExt != ".pdf")
                {
                    ModelState.AddModelError("BookUrl", "Only PDF files are allowed for the book.");
                    return View(book);
                }
                var bookFileName = $"bookfile_{Guid.NewGuid()}{bookExt}";
                var bookPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Books", bookFileName);
                Directory.CreateDirectory(Path.GetDirectoryName(bookPath));
                using (var stream = new FileStream(bookPath, FileMode.Create))
                {
                    await bookFile.CopyToAsync(stream);
                }
                book.BookUrl = $"/Books/{bookFileName}";
            }
            else
            {
                // Keep the original book file if no new file is uploaded
                book.BookUrl = originalBook.BookUrl;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Delete/5
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                // Delete image file if exists
                if (!string.IsNullOrEmpty(book.ImageUrl))
                {
                    // Support both /Books/ and /Images/Books/ paths
                    var imageRelativePath = book.ImageUrl.Replace("/", Path.DirectorySeparatorChar.ToString()).TrimStart(Path.DirectorySeparatorChar);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageRelativePath);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                // Delete book PDF file if exists
                if (!string.IsNullOrEmpty(book.BookUrl))
                {
                    var bookRelativePath = book.BookUrl.Replace("/", Path.DirectorySeparatorChar.ToString()).TrimStart(Path.DirectorySeparatorChar);
                    var bookPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", bookRelativePath);
                    if (System.IO.File.Exists(bookPath))
                    {
                        System.IO.File.Delete(bookPath);
                    }
                }
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
