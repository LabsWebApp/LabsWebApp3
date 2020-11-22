using System.ComponentModel.DataAnnotations;

namespace LabsWebApp5.Models.Identity
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
