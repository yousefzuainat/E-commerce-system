using ecommerce_system.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ecommerce_system.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ecommerce_system.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Admin");

                if (User.IsInRole("User"))
                    return RedirectToAction("Index", "User");
            }
            
            var products = await _context.proudects.Include(p => p.Category).ToListAsync();
            return View(products);
        }

       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
