using System.ComponentModel.DataAnnotations;

namespace SeboOnline.ViewModels.User;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "O campo nome é obrigatório")]
    [StringLength(40, ErrorMessage = "O campo nome deve ter no máximo 40 caracteres")]
    public string Name { get; set; }
    [Required(ErrorMessage = "O campo e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato do e-mail inválido")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O campo senha é obrigatório")]
    [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres")]
    public string Password { get; set; }
}