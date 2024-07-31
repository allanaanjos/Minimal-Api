using Microsoft.EntityFrameworkCore;
using MinimalApi.Data.Db;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.ViewModels;
using MinimalApi.Interface;

namespace MinimalApi.Domain.Services
{
    public class VeiculosServices : IVeiculosServices
    {
        private readonly AppDbContext _context;

        public VeiculosServices(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public Veiculos AtualizarVeiculo(int id, VeiculoViewModel model)
        {
            var veiculo = _context.Veiculos.Find(id);

            if (veiculo is null)
                throw new KeyNotFoundException($"Veículo com ID {id} não encontrado.");

            veiculo.Nome = model.Nome;
            veiculo.Marca = model.Marca;
            veiculo.Ano = model.Ano;

            _context.SaveChanges();

            return veiculo;
        }

        public Veiculos? BuscarVeiculoPorId(int id)
        {
            return _context.Veiculos.FirstOrDefault(x => x.id == id);
        }

        public Veiculos CriarVeiculo(Veiculos veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();

            return veiculo;
        }

        public Veiculos RemoverVeiculo(int id)
        {
            var data = _context.Veiculos.FirstOrDefault(x => x.id == id);


            if (data == null)
            {
                throw new KeyNotFoundException($"Veículo com ID {id} não encontrado.");
            }

            _context.Veiculos.Remove(data);
            _context.SaveChanges();

            return data;

        }

        public List<Veiculos> TodosOsVeiculos(int pagina = 1, string? nome = null, string? marca = null)
        {
            int TamanhoDaPagina = 10;

            var query = _context.Veiculos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {

                query = query.Where(x => x.Nome.Contains(nome.ToLower(), StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(marca))
            {

                query = query.Where(x => x.Marca.Contains(marca.ToLower(), StringComparison.OrdinalIgnoreCase));
            }

            return query
            .AsNoTracking()
            .Skip((pagina - 1) * TamanhoDaPagina)
            .Take(TamanhoDaPagina)
            .ToList();
        }
    }
}