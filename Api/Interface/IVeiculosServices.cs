using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ViewModels;

namespace MinimalApi.Interface
{
    public interface IVeiculosServices
    {
        List<Veiculos> TodosOsVeiculos(int pagina = 1, string? nome = null, string? marca = null);
        Veiculos? BuscarVeiculoPorId(int id);
        Veiculos CriarVeiculo(Veiculos veiculo);
        Veiculos AtualizarVeiculo(int id, VeiculoViewModel veiculo);
        Veiculos RemoverVeiculo(int id);

    }
}