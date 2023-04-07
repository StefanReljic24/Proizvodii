using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proizvodii.Data;
using Proizvodii.Entity;
using Proizvodii.Models;

namespace Proizvodii.Controllers
{
   
    
        [Authorize]
        public class ProductController : Controller
        {
            readonly EFDataContext _context;
            readonly string _imageFolderPath;
            public ProductController(EFDataContext context, IWebHostEnvironment env)
            {
                _context = context;

                _imageFolderPath = env.ContentRootPath + @"\wwwroot\images\";
            }

            public IActionResult Index([FromQuery] string filter, [FromQuery] int[] categories)
            {

                if (filter == null)
                    filter = "";
                var cats = _context.Category.ToList();
                IEnumerable<Product> product;


                if (string.IsNullOrEmpty(filter) && !categories.Any())
                {
                    product = _context.Product.ToList();
                }
                else
                {
                    product = _context.Product
                        .Where(p => p.Name.ToLower().Contains(filter.ToLower())
                        || p.Code.ToLower().Contains(filter.ToLower()));

                    product = product
                        .Where(p => !categories.Any()
                        || (p.Category != null && categories.Contains(p.Category.CategoryId)));
                }


                var categoryFilter = cats.Select(p => new CategoryFilter
                {
                    Id = p.CategoryId,
                    Name = p.Name,
                    Selected = categories.Contains(p.CategoryId)
                });

                var viewModel = new ProductListView
                {
                    CategoryFilter = categoryFilter,
                    Products = product,
                    Filter = filter
                };

                return View(viewModel);
            }

            public IActionResult Details(int id)
            {

                var product = _context.Product.Include(a => a.Category)
                    .SingleOrDefault(p => p.ProductId == id);
                return View(product);
            }

            public IActionResult Create()
            {

                var categories = _context.Category.ToList();

                ViewData["categories"] = new SelectList(categories, "CategoryId", "Name");
                return View("Edit", new Product());
            }

            public IActionResult Edit(int id)
            {
                var categories = _context.Category.ToList();
                ViewData["categories"] = new SelectList(categories, "CategoryId", "Name");
                return View(_context.Product.SingleOrDefault(p => p.ProductId == id));
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Product product)
            {
                try
                {

                    ModelState.Remove("Category.Name");
                    ModelState.Remove("Category.Code");

                    var categories = _context.Category.ToList();
                    ViewData["categories"] = new SelectList(categories, "CategoryId", "Name");


                    if (!ModelState.IsValid)
                        return View(product);



                    if (_context.Product.Any(p => p.Code == product.Code && p.ProductId != product.ProductId))
                    {

                        ModelState.AddModelError("Code", "Šifra već postoji u bazi");
                        return View(product);
                    }


                    if (product.ProductId == 0)
                    {
                        product.Category = _context.Category.Single(p => p.CategoryId == product.Category.CategoryId);
                        _context.Product.Add(product);
                    }
                    else
                    {
                        product.Category = null;
                        _context.Product.Update(product);
                    }


                    if (product.NewImage != null)
                    {
                        if (product.NewImage.Length > 0)
                        {
                            using (var stream = System.IO.File.Create(_imageFolderPath + product.NewImage.FileName))
                            {
                                product.NewImage.CopyTo(stream);
                            }
                        }
                        DeleteImage(product.ImageName);
                        product.ImageName = product.NewImage.FileName;
                    }

                    _context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(product);
                }
            }

            public IActionResult Delete(int id)
            {
                try
                {
                    var product = _context.Product.SingleOrDefault(p => p.ProductId == id);
                    if (product != null)
                    {
                        _context.Product.Remove(product);
                        _context.SaveChanges();

                        DeleteImage(product.ImageName);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            private void DeleteImage(string? imageName)
            {

                if (string.IsNullOrEmpty(imageName))
                    return;


                var fullImagePath = _imageFolderPath + imageName;

                if (System.IO.File.Exists(fullImagePath) && !imageName.EndsWith("_nd.jpg"))
                {

                    System.IO.File.Delete(fullImagePath);
                }
            }
        }
    
}
