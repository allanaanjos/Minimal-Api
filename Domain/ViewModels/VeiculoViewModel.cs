using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Domain.ViewModels
{
    public class VeiculoViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(150, ErrorMessage = "O nome deve ter no maxímo 150 caractere")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A marca é obrigatório")]
        [MaxLength(100, ErrorMessage = "A marca deve ter no maxímo 100 caractere")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Ano é obrigatório")]
        public int Ano { get; set; } 
    }
}