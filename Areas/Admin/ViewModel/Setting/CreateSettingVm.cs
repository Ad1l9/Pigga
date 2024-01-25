using System.ComponentModel.DataAnnotations;

namespace Pigga.Areas.Admin.ViewModel
{
    public class CreateSettingVm
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
