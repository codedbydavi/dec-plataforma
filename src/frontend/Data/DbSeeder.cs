using Frontend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is created and migrations are applied
            await context.Database.MigrateAsync();

            // Seed Roles (Matching AuthController IDs)
            if (!await context.RoleLookups.AnyAsync())
            {
                context.RoleLookups.AddRange(
                    new RoleLookup { Id = 1, Role = "Student" },
                    new RoleLookup { Id = 2, Role = "Professor" },
                    new RoleLookup { Id = 3, Role = "Admin" }
                );
                await context.SaveChangesAsync();
            }

            // Seed User Status
            if (!await context.UserStatuses.AnyAsync())
            {
                context.UserStatuses.AddRange(
                    new UserStatus { Id = 1, Status = "Active" },
                    new UserStatus { Id = 2, Status = "Inactive" },
                    new UserStatus { Id = 3, Status = "Banned" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Gender
            if (!await context.Genders.AnyAsync())
            {
                context.Genders.AddRange(
                    new Gender { Id = 1, Name = "Not Specified" },
                    new Gender { Id = 2, Name = "Male" },
                    new Gender { Id = 3, Name = "Female" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Class Status
            if (!await context.ClassStatuses.AnyAsync())
            {
                context.ClassStatuses.AddRange(
                    new ClassStatus { Id = 1, Status = "Active" },
                    new ClassStatus { Id = 2, Status = "Closed" },
                    new ClassStatus { Id = 3, Status = "Archived" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Entry Types (Portuguese)
            if (!await context.EntryTypes.AnyAsync())
            {
                context.EntryTypes.AddRange(
                    new EntryType { Type = "Rendimento" },
                    new EntryType { Type = "Despesa" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Categories (Detailed)
            if (!await context.Categories.AnyAsync())
            {
                context.Categories.AddRange(
                    new Category { Name = "Salário" },
                    new Category { Name = "Subsídio" },
                    new Category { Name = "Rendimentos Prediais" },
                    new Category { Name = "Outros Rendimentos" },
                    new Category { Name = "Habitação (Renda/Prestação)" },
                    new Category { Name = "Água" },
                    new Category { Name = "Luz" },
                    new Category { Name = "Gás" },
                    new Category { Name = "Telecomunicações" },
                    new Category { Name = "Alimentação (Supermercado)" },
                    new Category { Name = "Transporte (Combustível)" },
                    new Category { Name = "Transporte (Passe)" },
                    new Category { Name = "Saúde e Farmácia" },
                    new Category { Name = "Educação" },
                    new Category { Name = "Lazer e Restauração" },
                    new Category { Name = "Vestuário e Calçado" },
                    new Category { Name = "Seguros" },
                    new Category { Name = "Despesas Diversas" }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
