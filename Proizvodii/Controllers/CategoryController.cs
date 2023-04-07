using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proizvodii.Data;
using Proizvodii.Entity;

namespace Proizvodii.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        readonly EFDataContext _context;
        public CategoryController(EFDataContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromQuery] string filter)
        {
            ViewData["filter"] = filter;
            if (string.IsNullOrEmpty(filter))
                return View(_context.Category.ToList());
            else
            {
                var cat = _context.Category.Where(p => p.Name.ToLower().Contains(filter.ToLower())
                || p.Code.ToLower().Contains(filter.ToLower()));
                return View(cat.ToList());
            }
        }

        public IActionResult Create()
        {
            return View("Edit", new Category());
        }

        public IActionResult Edit(int id)
        {
            return View(_context.Category.SingleOrDefault(p => p.CategoryId == id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(category);

                if (category.CategoryId == 0)
                    _context.Category.Add(category);
                else
                    _context.Category.Update(category);

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var c = _context.Category.SingleOrDefault(p => p.CategoryId == id);
                if (c != null)
                {
                    _context.Category.Remove(c);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
