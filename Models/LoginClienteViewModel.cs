using System.ComponentModel.DataAnnotations;

namespace CupcakeStore.Models
{
public class LoginClienteViewModel
{
    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Informe o telefone")]
    public string Telefone { get; set; }
}
}