using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Domain.ViewModels
{
    public class CriarAdministradorViewModel
    {
        [Required(ErrorMessage = "Informe o Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o Senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o Perfil")]
        public string Perfil { get; set; } = string.Empty;
    }
}