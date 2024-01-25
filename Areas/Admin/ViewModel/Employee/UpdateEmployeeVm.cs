namespace Pigga.Areas.Admin.ViewModel
{
    public class UpdateEmployeeVm
    {
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string? FbLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? GoogleLink { get; set; }
        public string? InstaLink { get; set; }
    }
}
