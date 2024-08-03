using Microsoft.EntityFrameworkCore;
using MinimalApi.Domain.Entities;

namespace MinimalApi.Data.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Administrador> Administradores { get; set; } = null!;
        public DbSet<Veiculos> Veiculos {get; set;}= null!;


        protected override void OnModelCreating(ModelBuilder mb)
        {
            //Administrador
            mb.Entity<Administrador>().HasKey(x => x.Id);
            mb.Entity<Administrador>().Property(x => x.Email).HasMaxLength(150).IsRequired();
            mb.Entity<Administrador>().Property(x => x.Senha).HasMaxLength(30).IsRequired();
            mb.Entity<Administrador>().Property(x => x.Perfil).HasMaxLength(10).IsRequired();

            //veiculos
            mb.Entity<Veiculos>().HasKey(x => x.id);
            mb.Entity<Veiculos>().Property(x => x.Nome).HasMaxLength(150).IsRequired();
            mb.Entity<Veiculos>().Property(x => x.Marca).HasMaxLength(100).IsRequired();
            mb.Entity<Veiculos>().Property(x => x.Ano).IsRequired();

        }

    }
}