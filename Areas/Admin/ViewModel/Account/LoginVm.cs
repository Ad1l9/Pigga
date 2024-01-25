using System.ComponentModel.DataAnnotations;

namespace Pigga.Areas.Admin.ViewModel
{
    public class LoginVm
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
