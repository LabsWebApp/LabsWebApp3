using System.ComponentModel.DataAnnotations;

namespace LabsWebApp3.Models.IdentityModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин"), MaxLength(40), MinLength(4, ErrorMessage = "Слишком короткий логин (min > 4)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [UIHint("password")]
        [Display(Name = "Пароль"), MaxLength(40), MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }
}
