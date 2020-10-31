using System.ComponentModel.DataAnnotations;

namespace LabsWebApp3.Models.IdentityModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [ConcurrencyCheck]
        [Display(Name = "Логин")]
        [MaxLength(40, ErrorMessage = "Слишком длинный логин (max = 40)"), 
            MinLength(4, ErrorMessage = "Слишком короткий логин (min = 4)")]
        public string UserName { get; set; }

        [Required]
        [ConcurrencyCheck]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
