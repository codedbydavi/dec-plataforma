using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Frontend.Models.Entities;

namespace Frontend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Domain Entities
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Professor> Professors => Set<Professor>();
        public DbSet<Classroom> Classrooms => Set<Classroom>();
        public DbSet<Challenge> Challenges => Set<Challenge>();
        public DbSet<Scenario> Scenarios => Set<Scenario>();
        public DbSet<FinancialEntry> FinancialEntries => Set<FinancialEntry>();
        public DbSet<Income> Incomes => Set<Income>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Objective> Objectives => Set<Objective>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<SimulationHistory> SimulationHistories => Set<SimulationHistory>();
        public DbSet<ChallengeAssignment> ChallengeAssignments => Set<ChallengeAssignment>();

        // Lookup Entities
        public DbSet<RoleLookup> RoleLookups => Set<RoleLookup>();
        public DbSet<UserStatus> UserStatuses => Set<UserStatus>();
        public DbSet<ClassStatus> ClassStatuses => Set<ClassStatus>();
        public DbSet<Gender> Genders => Set<Gender>();
        public DbSet<EntryType> EntryTypes => Set<EntryType>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity Table Mapping (Aligning with MER 'Users')
            builder.Entity<ApplicationUser>(entity => {
                entity.ToTable("Users");
                entity.HasDiscriminator<string>("UserType")
                    .HasValue<Student>("Student")
                    .HasValue<Professor>("Professor")
                    .HasValue<ApplicationUser>("Base");
            });
            builder.Entity<IdentityRole<int>>().ToTable("Roles_Identity");

            // Custom Mappings to match MER.png exactly
            builder.Entity<Classroom>(entity => {
                entity.ToTable("Classes");
                entity.Property(e => e.MemberCode).HasColumnName("Member_code");
                entity.Property(e => e.TeacherId).HasColumnName("Teacher_id");
            });

            builder.Entity<Challenge>().ToTable("Challenges");

            builder.Entity<Enrollment>(entity => {
                entity.ToTable("Class_Students");
                entity.Property(e => e.ClassGroupId).HasColumnName("Class_id");
                entity.Property(e => e.StudentId).HasColumnName("Student_id");
            });

            builder.Entity<Scenario>(entity => {
                entity.ToTable("Scenarios");
                entity.Property(e => e.StudentId).HasColumnName("Student_id");
                entity.Property(e => e.FamilyName).HasColumnName("Family_Name");
                entity.Property(e => e.InitialBalance).HasColumnName("Initial_Balance");
            });

            builder.Entity<FinancialEntry>(entity => {
                entity.ToTable("Entries");
                entity.Property(e => e.ScenarioId).HasColumnName("Scenario_id");
                entity.Property(e => e.TypeId).HasColumnName("Type_id");
                entity.Property(e => e.CategoryId).HasColumnName("Category_id");
                
                // TPH for Entries
                entity.HasDiscriminator<string>("EntryClass")
                    .HasValue<Income>("Income")
                    .HasValue<Expense>("Expense");
            });

            builder.Entity<Objective>(entity => {
                entity.ToTable("Objectives");
                entity.Property(e => e.ScenarioId).HasColumnName("Scenario_id");
                entity.Property(e => e.TargetValue).HasColumnName("Target_Value");
                entity.Property(e => e.CurrentValue).HasColumnName("Current_Value");
                entity.Property(e => e.TargetMonths).HasColumnName("Target_Months");
            });

            builder.Entity<SimulationHistory>(entity => {
                entity.ToTable("Simulation_history");
                entity.Property(e => e.ScenarioId).HasColumnName("Scenario_id");
                entity.Property(e => e.ExecutionDate).HasColumnName("Execution_Date");
                entity.Property(e => e.FinalBalance).HasColumnName("Final_Balance");
                entity.Property(e => e.MonthsToGoal).HasColumnName("Months_to_Goal");
                entity.Property(e => e.EffortRate).HasColumnName("Effort_Rate");
                entity.Property(e => e.ResultsJson).HasColumnName("json_Results");
                entity.Property(e => e.Score).HasColumnName("Score");
                entity.Property(e => e.Feedback).HasColumnName("Feedback");
            });

            builder.Entity<ChallengeAssignment>(entity => {
                entity.ToTable("Class_Challenges");
                entity.Property(e => e.ClassroomId).HasColumnName("Class_id");
                entity.Property(e => e.ChallengeId).HasColumnName("Challenge_id");
            });

            // Lookups
            builder.Entity<RoleLookup>().ToTable("Roles");
            builder.Entity<UserStatus>().ToTable("userStatus");
            builder.Entity<ClassStatus>().ToTable("classStatus");
            builder.Entity<Gender>().ToTable("gender");
            builder.Entity<EntryType>().ToTable("Entry_types");
            builder.Entity<Category>().ToTable("Categories");
        }
    }
}
