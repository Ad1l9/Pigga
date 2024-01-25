using System.ComponentModel.DataAnnotations;

namespace Pigga.Areas.Admin.ViewModel
{
    public class CreateEmployeeVm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
        public string? FbLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? GoogleLink { get; set; }
        public string? InstaLink { get; set; }
    }
}
