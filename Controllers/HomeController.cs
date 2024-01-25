using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pigga.DAL;
using Pigga.ViewModel;
using System.Diagnostics;

namespace Pigga.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            HomeVm vm = new()
            {
                Employees = await _context.Employees.ToListAsync()
            };
            return View(vm);
        }
    }
}