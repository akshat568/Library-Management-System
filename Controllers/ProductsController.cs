using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAdmin.Data;
using ProductAdmin.Models;

namespace ProductAdmin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Product product,
            IFormFile? imageFile)
        {
            ModelState.Remove("ImagePath");

            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError(
                    "ImagePath",
                    "Product image is required.");
            }

            if (ModelState.IsValid)
            {
                string uploadDirectory = Path.Combine(
                    _environment.WebRootPath,
                    "uploads");

                Directory.CreateDirectory(uploadDirectory);

                string extension =
                    Path.GetExtension(imageFile!.FileName);

                string fileName =
                    Guid.NewGuid().ToString() + extension;

                string filePath =
                    Path.Combine(uploadDirectory, fileName);

                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImagePath =
                    "/uploads/" + fileName;

                _context.Add(product);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var product =
                await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product =
                await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            Product product,
            IFormFile? imageFile)
        {
            if (id != product.Id)
                return NotFound();

            ModelState.Remove("ImagePath");

            var existingProduct =
                await _context.Products.FindAsync(id);

            if (existingProduct == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingProduct.ProductName =
                    product.ProductName;

                existingProduct.ProductCategory =
                    product.ProductCategory;

                existingProduct.ProductFreshness =
                    product.ProductFreshness;

                existingProduct.AdditionalDescription =
                    product.AdditionalDescription;

                existingProduct.ProductPrice =
                    product.ProductPrice;

                existingProduct.MRP =
                    product.MRP;

                existingProduct.Comments =
                    product.Comments;

                existingProduct.ManagerEmail =
                    product.ManagerEmail;

                existingProduct.ManagerPhoneNumber =
                    product.ManagerPhoneNumber;

                if (imageFile != null &&
                    imageFile.Length > 0)
                {
                    string uploadDirectory =
                        Path.Combine(
                            _environment.WebRootPath,
                            "uploads");

                    Directory.CreateDirectory(uploadDirectory);

                    string extension =
                        Path.GetExtension(imageFile.FileName);

                    string fileName =
                        Guid.NewGuid().ToString() + extension;

                    string filePath =
                        Path.Combine(
                            uploadDirectory,
                            fileName);

                    using (var stream =
                           new FileStream(
                               filePath,
                               FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    if (!string.IsNullOrEmpty(
                        existingProduct.ImagePath))
                    {
                        string oldImage =
                            Path.Combine(
                                _environment.WebRootPath,
                                existingProduct.ImagePath
                                    .TrimStart('/'));

                        if (System.IO.File.Exists(oldImage))
                            System.IO.File.Delete(oldImage);
                    }

                    existingProduct.ImagePath =
                        "/uploads/" + fileName;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            product.ImagePath =
                existingProduct.ImagePath;

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product =
                await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            DeleteConfirmed(int id)
        {
            var product =
                await _context.Products.FindAsync(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    string imagePath =
                        Path.Combine(
                            _environment.WebRootPath,
                            product.ImagePath.TrimStart('/'));

                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}