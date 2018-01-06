using Confere.Processos.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Confere.Processos.Database
{
    public class ProcessoContext : DbContext
    {
        public DbSet<Processo> Processos { get; set; }
        public DbSet<Interessado> Interessados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Interesse>()
                .HasKey(i => new { i.InteressadoId, i.ProcessoId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connStringSection = ConfigurationManager.ConnectionStrings["processos"];
            var stringConexao = (connStringSection == null) 
                ? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Processos;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                : ConfigurationManager.ConnectionStrings["processos"].ConnectionString;
            optionsBuilder.UseSqlServer(stringConexao);
        }
    }
}
