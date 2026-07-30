using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly LibraryContext _context;

        public PublicationsController(LibraryContext context)
        {
            _context = context;
        }


        // =====================================================
        // INDEX
        // Newspaper / Magazine
        // Search + Pagination
        // =====================================================

        public async Task<IActionResult> Index(
            string type = "Newspaper",
            string? searchTerm = null,
            int page = 1)
        {
            // Convert URL string into PublicationType enum
            if (!Enum.TryParse<PublicationType>(
                    type,
                    true,
                    out var publicationType))
            {
                return NotFound();
            }


            const int pageSize = 5;

            if (page < 1)
            {
                page = 1;
            }


            // Only retrieve the requested publication type
            var query = _context.Publications
                .AsNoTracking()
                .Where(p => p.Type == publicationType);


            // =================================================
            // SEARCH
            // =================================================

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                query = query.Where(p =>
                    p.Title.Contains(searchTerm) ||
                    p.Publisher.Contains(searchTerm)
                );
            }


            // =================================================
            // PAGINATION
            // =================================================

            int totalItems =
                await query.CountAsync();


            int totalPages =
                (int)Math.Ceiling(
                    totalItems / (double)pageSize
                );


            if (totalPages > 0 &&
                page > totalPages)
            {
                page = totalPages;
            }


            var publications =
                await query
                    .OrderByDescending(p => p.PublishedDate)
                    .ThenBy(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


            var viewModel =
                new PublicationIndexViewModel
                {
                    Publications = publications,
                    Type = publicationType,
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


            var publication =
                await _context.Publications
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.Id == id);


            if (publication == null)
            {
                return NotFound();
            }


            return View(publication);
        }


        // =====================================================
        // CREATE - GET
        // =====================================================

        public IActionResult Create(
            string type = "Newspaper")
        {
            if (!Enum.TryParse<PublicationType>(
                    type,
                    true,
                    out var publicationType))
            {
                return NotFound();
            }


            var publication =
                new Publication
                {
                    Type = publicationType,
                    PublishedDate = DateTime.Today,
                    IsAvailable = true
                };


            return View(publication);
        }


        // =====================================================
        // CREATE - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Publication publication)
        {
            if (!ModelState.IsValid)
            {
                return View(publication);
            }


            _context.Publications.Add(publication);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"{publication.Type} '{publication.Title}' was added successfully.";


            return RedirectToAction(
                nameof(Index),
                new
                {
                    type = publication.Type.ToString()
                });
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


            var publication =
                await _context.Publications
                    .FindAsync(id);


            if (publication == null)
            {
                return NotFound();
            }


            return View(publication);
        }


        // =====================================================
        // EDIT - POST
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Publication publication)
        {
            if (id != publication.Id)
            {
                return NotFound();
            }


            if (!ModelState.IsValid)
            {
                return View(publication);
            }


            try
            {
                _context.Update(publication);

                await _context.SaveChangesAsync();


                TempData["SuccessMessage"] =
                    $"{publication.Type} '{publication.Title}' was updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists(publication.Id))
                {
                    return NotFound();
                }

                throw;
            }


            return RedirectToAction(
                nameof(Index),
                new
                {
                    type = publication.Type.ToString()
                });
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


            var publication =
                await _context.Publications
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p =>
                        p.Id == id);


            if (publication == null)
            {
                return NotFound();
            }


            return View(publication);
        }


        // =====================================================
        // DELETE - POST
        // =====================================================

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(
            int id)
        {
            var publication =
                await _context.Publications
                    .FindAsync(id);


            if (publication == null)
            {
                return NotFound();
            }


            var publicationType =
                publication.Type;


            var publicationTitle =
                publication.Title;


            _context.Publications.Remove(publication);

            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] =
                $"{publicationType} '{publicationTitle}' was deleted successfully.";


            return RedirectToAction(
                nameof(Index),
                new
                {
                    type = publicationType.ToString()
                });
        }


        // =====================================================
        // EXISTS
        // =====================================================

        private bool PublicationExists(int id)
        {
            return _context.Publications
                .Any(p => p.Id == id);
        }
    }
}