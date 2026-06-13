using Microsoft.EntityFrameworkCore;
using DEC.Models;

namespace DEC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Simulacao> Simulacoes { get; set; }
        public DbSet<Desafio> Desafios { get; set; }
        public DbSet<DesafioAluno> DesafiosAluno { get; set; }
        public DbSet<Badge> Badges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Familias)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Familia>()
                .HasMany(f => f.Transacoes)
                .WithOne(t => t.Familia)
                .HasForeignKey(t => t.FamiliaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Familia>()
                .HasMany(f => f.Simulacoes)
                .WithOne(s => s.Familia)
                .HasForeignKey(s => s.FamiliaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.DesafiosCriados)
                .WithOne(d => d.CriadoPor)
                .HasForeignKey(d => d.CriadoPorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data inicial
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Nome = "Admin ISTEC",
                    Email = "admin@istec.pt",
                    Password = "admin123", // Em produção, usar hash
                    Role = UserRole.Administrador,
                    DataCriacao = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Nome = "Prof. Silva",
                    Email = "prof.silva@istec.pt",
                    Password = "prof123",
                    Role = UserRole.Professor,
                    DataCriacao = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    Nome = "João Aluno",
                    Email = "joao@istec.pt",
                    Password = "aluno123",
                    Role = UserRole.Aluno,
                    Turma = "10A",
                    DataCriacao = DateTime.Now
                }
            );

            modelBuilder.Entity<Badge>().HasData(
                new Badge { Id = 1, Nome = "Primeira Família", Descricao = "Criou a primeira família", Icone = "🏠", PontosNecessarios = 10 },
                new Badge { Id = 2, Nome = "Poupador Iniciante", Descricao = "Atingiu 500€ de poupança", Icone = "💰", PontosNecessarios = 50 },
                new Badge { Id = 3, Nome = "Gestor Expert", Descricao = "Completou 10 desafios", Icone = "🏆", PontosNecessarios = 100 },
                new Badge { Id = 4, Nome = "Mestre Financeiro", Descricao = "Taxa de esforço abaixo de 30%", Icone = "⭐", PontosNecessarios = 150 }
            );
        }
    }
}
