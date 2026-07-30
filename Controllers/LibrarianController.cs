using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class LibrarianController : Controller
    {
        private readonly LibraryContext _context;

        public LibrarianController(LibraryContext context)
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

            var librariansQuery = _context.Librarians
                .AsNoTracking()
                .AsQueryable();


            // -----------------------------
            // SEARCH
            // -----------------------------

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                librariansQuery = librariansQuery.Where(l =>
                    l.Name.Contains(searchTerm)
                );
            }


            // -----------------------------
            // PAGINATION
            // -----------------------------

            int totalItems =
                await librariansQuery.CountAsync();

            int totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize
                );


            if (totalPages > 0 &&
                page > totalPages)
            {
                page = totalPages;
            }


            var librarians =
                await librariansQuery
                    .OrderBy(l => l.LibrarianId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


            var viewModel =
                new LibrarianIndexViewModel
                {
                    Librarians = librarians,
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


            var librarian =
                await _context.Librarians
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l =>
                        l.LibrarianId == id);


            if (librarian == null)
            {
                return NotFound();
            }


            return View(librarian);
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
        public async Task<IActionResult> Create(
            Librarian librarian)
        {
            if (!ModelState.IsValid)
            {
                return View(librarian);
            }


            _context.Librarians.Add(librarian);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"Librarian '{librarian.Name}' was added successfully.";


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


            var librarian =
                await _context.Librarians
                    .FindAsync(id);


            if (librarian == null)
            {
                return NotFound();
            }


            return View(librarian);
        }


        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Librarian librarian)
        {
            if (id != librarian.LibrarianId)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(librarian);
            }


            try
            {
                _context.Update(librarian);

                await _context.SaveChangesAsync();


                TempData["SuccessMessage"] =
                    $"Librarian '{librarian.Name}' was updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibrarianExists(
                    librarian.LibrarianId))
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


            var librarian =
                await _context.Librarians
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l =>
                        l.LibrarianId == id);


            if (librarian == null)
            {
                return NotFound();
            }


            return View(librarian);
        }


        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var librarian =
                await _context.Librarians
                    .FindAsync(id);


            if (librarian == null)
            {
                return NotFound();
            }


            _context.Librarians.Remove(librarian);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"Librarian '{librarian.Name}' was deleted successfully.";


            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // LIBRARIAN EXISTS
        // =====================================================

        private bool LibrarianExists(int id)
        {
            return _context.Librarians
                .Any(l => l.LibrarianId == id);
        }
    }
}