using System.ComponentModel.DataAnnotations;

namespace LabsWebApp3.Models.Identity
{
    public class EditRegisterModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        [MaxLength(40, ErrorMessage = "Слишком длинный логин (max = 40)"),
         MinLength(4, ErrorMessage = "Слишком короткий логин (min = 4)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Введите Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
