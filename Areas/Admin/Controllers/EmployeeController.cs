using Microsoft.AspNetCore.Mvc;

namespace Pigga.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
