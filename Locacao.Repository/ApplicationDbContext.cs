using Locacao.Service.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Locacao.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }
        public DbSet<Moto> Motos { get; set; }
        public DbSet<Entregador> Entregadores { get; set; }
        public DbSet<LocacaoMoto> Locacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Moto>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Placa).IsRequired().HasMaxLength(8);
                entity.Property(m => m.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(m => m.Ano).IsRequired();
            });

            modelBuilder.Entity<Entregador>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Cnpj).IsRequired().HasMaxLength(14);
                entity.HasIndex(e => e.Cnpj).IsUnique();
                entity.Property(e => e.NumeroCnh).IsRequired().HasMaxLength(11);
                entity.Property(e => e.UrlCnh).HasMaxLength(200);
            });

            modelBuilder.Entity<LocacaoMoto>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.HasOne(l => l.Moto)
                    .WithMany(m => m.Locacoes)
                    .HasForeignKey(l => l.MotoId);

                entity.HasOne(l => l.Entregador)
                    .WithMany(e => e.Locacoes)
                    .HasForeignKey(l => l.EntregadorId);

                entity.Property(l => l.DataInicio).IsRequired();
                entity.Property(l => l.DataTerminoPrevisto).IsRequired();
                entity.Property(l => l.DataTermino).IsRequired(false); 
            });

        }

    }

}
