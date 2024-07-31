using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApi.Domain.Entities
{
    public class Administrador
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o email")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha")]
        [StringLength(50)]
        public string Senha { get; set; } = string.Empty;

        [StringLength(10)]
        public string Perfil { get; set; } = string.Empty;
    }
}