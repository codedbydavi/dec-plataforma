using System.ComponentModel.DataAnnotations;

namespace Frontend.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O nome de utilizador é obrigatório")]
        [Display(Name = "Utilizador")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "A palavra-passe é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra-passe")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}
