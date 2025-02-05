using DevSteamAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSteamAPI.Data
{
    public class DevSteamAPIContext : IdentityDbContext
    {
        //Metodo Cronsutor
        public DevSteamAPIContext(DbContextOptions<DevSteamAPIContext> options) : base(options) { }
        //Defifnir as tabelas do banco de dados
        //Maneira em que a model será representada da base de dados
        public DbSet<Jogo> Jogos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        //Sobrescrever o metodo OnModelCreating
        //Para definir o nome das tabelas;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Jogo>().ToTable("Jogos");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
        }


    }
}
