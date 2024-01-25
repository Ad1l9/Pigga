using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pigga.Areas.Admin.ViewModel;
using Pigga.DAL;
using Pigga.Models;
using Pigga.Utilities.Extension;

namespace Pigga.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _context.Employees.ToListAsync();
            return View(employees);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeVm vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            if (!vm.Photo.ValidateSize(2))
            {
                ModelState.AddModelError("Photo", "Image size is invalid");
                return View(vm);
            }
            if (!vm.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "Image type is invalid");
                return View(vm);
            }
            string fileName = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "imgs");
            Employee employee = new()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                Description = vm.Description,
                ImageUrl = fileName,
                FbLink = vm.FbLink,
                TwitterLink = vm.TwitterLink,
                InstaLink = vm.InstaLink,
                GoogleLink = vm.GoogleLink
            };
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
