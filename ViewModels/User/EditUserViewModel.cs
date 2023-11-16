using System.ComponentModel.DataAnnotations;

namespace SeboOnline.ViewModels.User;

public class EditUserViewModel
{
    [StringLength(40, ErrorMessage = "O campo nome deve ter no máximo 40 caracteres")]
    public string Name { get; set; }
    [EmailAddress(ErrorMessage = "Formato do e-mail inválido")]
    public string Email { get; set; }
}