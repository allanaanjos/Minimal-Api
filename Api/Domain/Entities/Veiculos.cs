namespace MinimalApi.Domain.Entities
{
    public class Veiculos
    {
        public int id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int Ano { get; set; } 
    }
}