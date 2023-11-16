using System.ComponentModel.DataAnnotations;

namespace SeboOnline.ViewModels.User;

public class LoginViewModel
{
    [Required(ErrorMessage = "Campo de E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de E-mail inválido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Campo de Senha é obrigatório")]
    public string Password { get; set; }
}