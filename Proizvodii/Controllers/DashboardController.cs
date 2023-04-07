using Microsoft.AspNetCore.Mvc;
using Proizvodii.Data;

namespace Proizvodii.Controllers
{
    public class DashboardController : Controller
    {
        EFDataContext _context;
        public DashboardController(EFDataContext context)
        {
            _context = context;
        }

        public IActionResult TopProducts()
        {

            var products = _context.Product
                .OrderBy(p => Guid.NewGuid())
                .Take(3);

            return PartialView(products);
        }

        public IActionResult Counters()
        {
            ViewData["userCount"] = _context.User.Count();
            ViewData["productCount"] = _context.Product.Count();

            return PartialView();
        }
    }
}
