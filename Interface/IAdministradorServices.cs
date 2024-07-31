using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ViewModels;
using MinimalApi.ViewModels;

namespace MinimalApi.Interface
{
    public interface IAdministradorServices
    {
        Administrador? Login(LoginViewModel model);
        Administrador Criar(CriarAdministradorViewModel model);
        List<Administrador> Todos();
        Administrador BuscarPorId(int id);
        Administrador Remover(int id);
    }
}