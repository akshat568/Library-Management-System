using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibraryContext _context;

        public HomeController(LibraryContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            // ============================================
            // BOOK STATISTICS
            // ============================================

            var totalBooks =
                await _context.Books12.CountAsync();


            var availableBooks =
                await _context.Books12
                    .CountAsync(b => b.IsAvailable);


            var borrowedBooks =
                await _context.Books12
                    .CountAsync(b => !b.IsAvailable);


            // ============================================
            // BORROW STATISTICS
            // ============================================

            var totalBorrowRecords =
                await _context.BorrowRecords12.CountAsync();


            // ============================================
            // NEW MODULE STATISTICS
            // ============================================

            var totalStudents =
                await _context.Students.CountAsync();


            var totalLibrarians =
                await _context.Librarians.CountAsync();


            var totalNewspapers =
                await _context.Publications
                    .CountAsync(
                        p => p.Type ==
                             PublicationType.Newspaper
                    );


            var totalMagazines =
                await _context.Publications
                    .CountAsync(
                        p => p.Type ==
                             PublicationType.Magazine
                    );


            // ============================================
            // AVAILABILITY PERCENTAGE
            // ============================================

            var availabilityPercentage =
                totalBooks == 0
                    ? 0
                    : (int)Math.Round(
                        (double)availableBooks /
                        totalBooks * 100
                    );


            // ============================================
            // RECENT BOOKS
            // ============================================

            var recentBooks =
                await _context.Books12
                    .AsNoTracking()
                    .OrderByDescending(b => b.BookId)
                    .Take(5)
                    .ToListAsync();


            // ============================================
            // ACTIVE BORROWS
            // ============================================

            var activeBorrows =
                await _context.BorrowRecords12
                    .AsNoTracking()
                    .Include(r => r.Book)
                    .Where(r => r.ReturnDate == null)
                    .OrderByDescending(r => r.BorrowDate)
                    .Take(5)
                    .ToListAsync();


            // ============================================
            // RECENT BORROWING ACTIVITY
            // ============================================

            var recentActivity =
                await _context.BorrowRecords12
                    .AsNoTracking()
                    .Include(r => r.Book)
                    .OrderByDescending(
                        r => r.ReturnDate ?? r.BorrowDate
                    )
                    .Take(5)
                    .ToListAsync();


            // ============================================
            // DASHBOARD VIEW MODEL
            // ============================================

            var viewModel =
                new DashboardViewModel
                {
                    // Books
                    TotalBooks = totalBooks,
                    AvailableBooks = availableBooks,
                    BorrowedBooks = borrowedBooks,

                    // Borrowing
                    TotalBorrowRecords =
                        totalBorrowRecords,

                    AvailabilityPercentage =
                        availabilityPercentage,

                    // New modules
                    TotalStudents =
                        totalStudents,

                    TotalLibrarians =
                        totalLibrarians,

                    TotalNewspapers =
                        totalNewspapers,

                    TotalMagazines =
                        totalMagazines,

                    // Dashboard data
                    RecentBooks =
                        recentBooks,

                    ActiveBorrows =
                        activeBorrows,

                    RecentActivity =
                        recentActivity
                };


            return View(viewModel);
        }
    }
}