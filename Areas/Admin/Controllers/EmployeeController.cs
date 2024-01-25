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





        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Employee existed=await _context.Employees.FirstOrDefaultAsync(e=>e.Id == id);
            if(existed is null) return NotFound();

            UpdateEmployeeVm vm = new()
            {
                Name = existed.Name,
                Surname = existed.Surname,
                Description = existed.Description,
                ImageUrl = existed.ImageUrl,
                TwitterLink = existed.TwitterLink,
                InstaLink = existed.InstaLink,
                GoogleLink = existed.GoogleLink,
                FbLink = existed.FbLink,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateEmployeeVm vm)
        {
            Employee existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            vm.ImageUrl = existed.ImageUrl;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if(vm.Photo is not null)
            {
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
                existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "imgs");
                string fileName = await vm.Photo.CreateFileAsync(_env.WebRootPath, "assets", "imgs");
                existed.ImageUrl= fileName;
            }
            existed.TwitterLink = vm.TwitterLink;
            existed.GoogleLink = vm.GoogleLink;
            existed.FbLink = vm.FbLink;
            existed.InstaLink = vm.InstaLink;

            existed.UpdatedAt = DateTime.Now;
            existed.Description = vm.Description;
            existed.Name = vm.Name;
            existed.Surname = vm.Surname;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Employee existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();

            existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "imgs");
            _context.Remove(existed);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            Employee existed = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }
    }
}
