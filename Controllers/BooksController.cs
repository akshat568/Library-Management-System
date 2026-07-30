using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        // GET: Books
        // Supports searching and pagination
        public async Task<IActionResult> Index(
            string? searchQuery,
            int page = 1)
        {
            try
            {
                const int pageSize = 5;

                // Prevent invalid page numbers
                if (page < 1)
                {
                    page = 1;
                }

                // Start building the query.
                // Nothing is loaded from the database yet.
                var booksQuery = _context.Books12
                    .Include(b => b.BorrowRecords)
                    .AsNoTracking()
                    .AsQueryable();

                // -------------------------
                // SEARCHING
                // -------------------------
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    searchQuery = searchQuery.Trim();

                    booksQuery = booksQuery.Where(b =>
                        (b.Title != null &&
                         b.Title.Contains(searchQuery)) ||

                        (b.Author != null &&
                         b.Author.Contains(searchQuery)) ||

                        (b.ISBN != null &&
                         b.ISBN.Contains(searchQuery))
                    );
                }

                // -------------------------
                // PAGINATION
                // -------------------------

                // Number of records AFTER applying search
                int totalItems = await booksQuery.CountAsync();

                int totalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize
                );

                // If user manually enters a page greater
                // than the available pages
                if (totalPages > 0 && page > totalPages)
                {
                    page = totalPages;
                }

                // Always order before Skip/Take
                var books = await booksQuery
                    .OrderBy(b => b.BookId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // -------------------------
                // CREATE VIEW MODEL
                // -------------------------
                var viewModel = new BookListViewModel
                {
                    Books = books,
                    SearchQuery = searchQuery,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };

                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while loading the books.";

                return View("Error");
            }
        }
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.ErrorMessage = "Book ID was not provided.";
                return View("NotFound");
            }

            try
            {
                var book = await _context.Books12
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BookId == id);

                if (book == null)
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id}.";

                    return View("NotFound");
                }

                return View(book);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while loading the book details.";

                return View("Error");
            }
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                book.IsAvailable = true;

                _context.Books12.Add(book);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Successfully added the book: {book.Title}.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while adding the book."
                );

                return View(book);
            }
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.ErrorMessage =
                    "Book ID was not provided for editing.";

                return View("NotFound");
            }

            try
            {
                var book = await _context.Books12
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BookId == id);

                if (book == null)
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id} for editing.";

                    return View("NotFound");
                }

                return View(book);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while loading the book for editing.";

                return View("Error");
            }
        }

        // POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id <= 0)
            {
                ViewBag.ErrorMessage =
                    "Book ID was not provided for updating.";

                return View("NotFound");
            }

            if (!ModelState.IsValid)
            {
                return View(book);
            }

            try
            {
                var existingBook =
                    await _context.Books12.FindAsync(id);

                if (existingBook == null)
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id} for updating.";

                    return View("NotFound");
                }

                // Only update fields the administrator
                // is allowed to modify.
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.ISBN = book.ISBN;
                existingBook.PublishedDate = book.PublishedDate;

                // Do NOT update IsAvailable here.

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Successfully updated the book: {book.Title}.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id}.";

                    return View("NotFound");
                }

                TempData["ErrorMessage"] =
                    "A concurrency error occurred during the update.";

                return View("Error");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while updating the book.";

                return View("Error");
            }
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                ViewBag.ErrorMessage =
                    "Book ID was not provided for deletion.";

                return View("NotFound");
            }

            try
            {
                var book = await _context.Books12
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.BookId == id);

                if (book == null)
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id} for deletion.";

                    return View("NotFound");
                }

                return View(book);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while loading the book for deletion.";

                return View("Error");
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var book = await _context.Books12
                    .FindAsync(id);

                if (book == null)
                {
                    ViewBag.ErrorMessage =
                        $"No book found with ID {id} for deletion.";

                    return View("NotFound");
                }

                _context.Books12.Remove(book);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Successfully deleted the book: {book.Title}.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while deleting the book.";

                return View("Error");
            }
        }

        private bool BookExists(int id)
        {
            return _context.Books12.Any(
                b => b.BookId == id
            );
        }
    }
}