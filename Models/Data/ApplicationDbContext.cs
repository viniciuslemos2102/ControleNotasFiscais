using Microsoft.EntityFrameworkCore;
using ControleNotasFiscais.Models;

namespace ControleNotasFiscais.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
    }
}
