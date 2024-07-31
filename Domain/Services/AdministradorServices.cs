using Microsoft.EntityFrameworkCore;
using MinimalApi.Data.Db;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ViewModels;
using MinimalApi.Interface;
using MinimalApi.ViewModels;

namespace MinimalApi.Domain.Services
{
    public class AdministradorServices : IAdministradorServices
    {
        private readonly AppDbContext context;
        public AdministradorServices(AppDbContext appDbContext)
        {
            context = appDbContext;
        }

        public Administrador? Login(LoginViewModel model)
        {
            var adm = context.Administradores.Where(x => x.Email == model.Email && x.Senha == model.Passworld).FirstOrDefault();
            return adm;
        }

        public Administrador Criar(CriarAdministradorViewModel model)
        {
            if(model is null)
               throw new InvalidOperationException("Administrador Inválido");

            var adm = new Administrador
            {
                Email = model.Email,
                Senha = model.Senha,
                Perfil = model.Perfil
            };

            context.Administradores.Add(adm);
            context.SaveChanges();
            return adm;
                
        }

        public List<Administrador> Todos()
        {
            return context.Administradores.AsNoTracking().ToList();
        }

        public Administrador BuscarPorId(int id)
        {
           var data = context.Administradores.Find(id);

           if(data is null)
              throw new Exception("Administrador não encontrado");
              
           return data;
        }

        public Administrador Remover(int id)
        {
           var data = context.Administradores.Find(id);

           if(data is null)
             throw new Exception("O administrador não encontrado");

           context.Administradores.Remove(data);
           context.SaveChanges();

           return data;
        }
    }
}