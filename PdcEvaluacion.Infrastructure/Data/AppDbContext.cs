using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PdcEvaluacion.Core.Entities;

namespace PdcEvaluacion.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pais> Paises { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }

        public DbSet<ColaboradorEmpresa> ColaboradoresEmpresas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ColaboradorEmpresa>()
                .HasKey(ce => new { ce.ColaboradorId, ce.EmpresaId });

            modelBuilder.Entity<ColaboradorEmpresa>()
                .HasOne(ce => ce.Colaborador)
                .WithMany(c => c.ColaboradoresEmpresas)
                .HasForeignKey(ce => ce.ColaboradorId);

            modelBuilder.Entity<ColaboradorEmpresa>()
                .HasOne(ce => ce.Empresa)
                .WithMany(e => e.ColaboradoresEmpresas)
                .HasForeignKey(ce => ce.EmpresaId);

        }
    }
}
