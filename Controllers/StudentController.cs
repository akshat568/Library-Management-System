using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly LibraryContext _context;

        public StudentController(LibraryContext context)
        {
            _context = context;
        }


        // =====================================================
        // INDEX
        // Search + Pagination
        // =====================================================

        public async Task<IActionResult> Index(
            string? searchTerm,
            int page = 1)
        {
            const int pageSize = 5;

            if (page < 1)
            {
                page = 1;
            }

            var studentsQuery = _context.Students
                .AsNoTracking()
                .AsQueryable();


            // -----------------------------
            // SEARCH
            // -----------------------------

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                studentsQuery = studentsQuery.Where(s =>
                    s.StudentName.Contains(searchTerm) ||
                    s.Email.Contains(searchTerm) ||
                    s.Phone.Contains(searchTerm)
                );
            }


            // -----------------------------
            // PAGINATION
            // -----------------------------

            int totalItems = await studentsQuery.CountAsync();

            int totalPages = (int)Math.Ceiling(
                totalItems / (double)pageSize
            );


            if (totalPages > 0 && page > totalPages)
            {
                page = totalPages;
            }


            var students = await studentsQuery
                .OrderBy(s => s.StudentId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            var viewModel = new StudentIndexViewModel
            {
                Students = students,
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems
            };


            return View(viewModel);
        }


        // =====================================================
        // DETAILS
        // =====================================================

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // =====================================================
        // CREATE - GET
        // =====================================================

        public IActionResult Create()
        {
            return View();
        }


        // =====================================================
        // CREATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            _context.Students.Add(student);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                $"Student '{student.StudentName}' was added successfully.";

            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // EDIT - GET
        // =====================================================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(student);
            }


            try
            {
                _context.Update(student);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Student '{student.StudentName}' was updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentId))
                {
                    return NotFound();
                }

                throw;
            }


            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // DELETE - GET
        // =====================================================

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }


            return View(student);
        }


        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students
                .FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }


            _context.Students.Remove(student);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"Student '{student.StudentName}' was deleted successfully.";


            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // STUDENT EXISTS
        // =====================================================

        private bool StudentExists(int id)
        {
            return _context.Students
                .Any(s => s.StudentId == id);
        }
    }
}