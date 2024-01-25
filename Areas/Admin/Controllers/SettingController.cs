using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pigga.Areas.Admin.ViewModel;
using Pigga.DAL;
using Pigga.Models;
using Pigga.Utilities.Extension;

namespace Pigga.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Setting existed = await _context.Settings.FirstOrDefaultAsync(e => e.Id == id);
            if (existed is null) return NotFound();

            UpdateSettingVm vm = new()
            {
                Key=existed.Key,
                Value=existed.Value
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateSettingVm vm)
        {
            Setting existed = await _context.Settings.FirstOrDefaultAsync(e => e.Id == id);
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            existed.Key = vm.Key;
            existed.Value = vm.Value;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
