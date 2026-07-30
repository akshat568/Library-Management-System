using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagement.Controllers
{
    [Authorize]
    public class BorrowController : Controller
    {
        private readonly LibraryContext _context;

        public BorrowController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Borrow/Create?bookId=1
        public async Task<IActionResult> Create(int bookId)
        {
            var book = await _context.Books12
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (book == null)
            {
                ViewBag.ErrorMessage =
                    $"No book found with ID {bookId}.";

                return View("NotFound");
            }

            if (!book.IsAvailable)
            {
                TempData["ErrorMessage"] =
                    "This book is currently unavailable.";

                return RedirectToAction(
                    "Index",
                    "Books"
                );
            }

            var borrowRecord = new BorrowRecord
            {
                BookId = book.BookId
            };

            ViewBag.BookTitle = book.Title;

            return View(borrowRecord);
        }

        // POST: Borrow/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BorrowRecord borrowRecord)
        {
            if (!ModelState.IsValid)
            {
                var currentBook =
                    await _context.Books12
                        .AsNoTracking()
                        .FirstOrDefaultAsync(
                            b => b.BookId == borrowRecord.BookId
                        );

                ViewBag.BookTitle = currentBook?.Title;

                return View(borrowRecord);
            }

            try
            {
                var book =
                    await _context.Books12
                        .FirstOrDefaultAsync(
                            b => b.BookId == borrowRecord.BookId
                        );

                if (book == null)
                {
                    ViewBag.ErrorMessage =
                        "The selected book could not be found.";

                    return View("NotFound");
                }

                if (!book.IsAvailable)
                {
                    ModelState.AddModelError(
                        "",
                        "This book has already been borrowed."
                    );

                    ViewBag.BookTitle = book.Title;

                    return View(borrowRecord);
                }

                borrowRecord.BorrowDate = DateTime.UtcNow;

                book.IsAvailable = false;

                _context.BorrowRecords12.Add(borrowRecord);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    $"Successfully borrowed: {book.Title}.";

                return RedirectToAction(
                    "Index",
                    "Books"
                );
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while borrowing the book."
                );

                return View(borrowRecord);
            }
        }

        // GET: Borrow/Return?borrowRecordId=1
        public async Task<IActionResult> Return(
            int borrowRecordId)
        {
            var record =
                await _context.BorrowRecords12
                    .Include(r => r.Book)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        r => r.BorrowRecordId == borrowRecordId
                    );

            if (record == null)
            {
                ViewBag.ErrorMessage =
                    "Borrow record could not be found.";

                return View("NotFound");
            }

            if (record.ReturnDate != null)
            {
                TempData["ErrorMessage"] =
                    "This book has already been returned.";

                return RedirectToAction(
                    "Index",
                    "Books"
                );
            }

            return View(record);
        }

        // POST: Borrow/ReturnConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReturnConfirmed(
            int borrowRecordId)
        {
            try
            {
                var record =
                    await _context.BorrowRecords12
                        .Include(r => r.Book)
                        .FirstOrDefaultAsync(
                            r => r.BorrowRecordId == borrowRecordId
                        );

                if (record == null)
                {
                    ViewBag.ErrorMessage =
                        "Borrow record could not be found.";

                    return View("NotFound");
                }

                if (record.ReturnDate != null)
                {
                    TempData["ErrorMessage"] =
                        "This book has already been returned.";

                    return RedirectToAction(
                        "Index",
                        "Books"
                    );
                }

                record.ReturnDate = DateTime.UtcNow;

                if (record.Book != null)
                {
                    record.Book.IsAvailable = true;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Book returned successfully.";

                return RedirectToAction(
                    "Index",
                    "Books"
                );
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] =
                    "An error occurred while returning the book.";

                return RedirectToAction(
                    "Index",
                    "Books"
                );
            }
        }
    }
}