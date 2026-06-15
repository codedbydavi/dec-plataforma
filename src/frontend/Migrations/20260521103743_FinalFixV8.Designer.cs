
using System;
using Frontend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Frontend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260521103743_FinalFixV8")]
    partial class FinalFixV8
    {

        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Frontend.Models.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("GenderId")
                        .HasColumnType("int");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("UserStatusId")
                        .HasColumnType("int");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserStatusId");

                    b.ToTable("Users", (string)null);

                    b.HasDiscriminator<string>("UserType").HasValue("Base");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Frontend.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Challenge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessLink")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.ToTable("Challenges", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.ClassStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("classStatus", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Classroom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MemberCode")
                        .HasColumnType("int")
                        .HasColumnName("Member_code");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int")
                        .HasColumnName("Teacher_id");

                    b.HasKey("Id");

                    b.HasIndex("StatusId");

                    b.HasIndex("TeacherId");

                    b.ToTable("Classes", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClassGroupId")
                        .HasColumnType("int")
                        .HasColumnName("Class_id");

                    b.Property<DateTime>("EnrolledAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("Student_id");

                    b.HasKey("Id");

                    b.HasIndex("ClassGroupId");

                    b.HasIndex("StudentId");

                    b.ToTable("Class_Students", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.EntryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Entry_types", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.FinancialEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("Category_id");

                    b.Property<string>("EntryClass")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("varchar(21)");

                    b.Property<int?>("EntryTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Recurrence")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("int")
                        .HasColumnName("Scenario_id");

                    b.Property<int>("TypeId")
                        .HasColumnType("int")
                        .HasColumnName("Type_id");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EntryTypeId");

                    b.HasIndex("ScenarioId");

                    b.ToTable("Entries", (string)null);

                    b.HasDiscriminator<string>("EntryClass").HasValue("FinancialEntry");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Frontend.Models.Entities.Gender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("gender", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Objective", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("CurrentValue")
                        .HasColumnType("float")
                        .HasColumnName("Current_Value");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("int")
                        .HasColumnName("Scenario_id");

                    b.Property<int>("TargetMonths")
                        .HasColumnType("int")
                        .HasColumnName("Target_Months");

                    b.Property<float>("TargetValue")
                        .HasColumnType("float")
                        .HasColumnName("Target_Value");

                    b.HasKey("Id");

                    b.HasIndex("ScenarioId");

                    b.ToTable("Objectives", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.RoleLookup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Scenario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Family_Name");

                    b.Property<float>("InitialBalance")
                        .HasColumnType("float")
                        .HasColumnName("Initial_Balance");

                    b.Property<int>("StudentId")
                        .HasColumnType("int")
                        .HasColumnName("Student_id");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.ToTable("Scenarios", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.SimulationHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("EffortRate")
                        .HasColumnType("float")
                        .HasColumnName("Effort_Rate");

                    b.Property<DateTime>("ExecutionDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Execution_Date");

                    b.Property<float>("FinalBalance")
                        .HasColumnType("float")
                        .HasColumnName("Final_Balance");

                    b.Property<int>("MonthsToGoal")
                        .HasColumnType("int")
                        .HasColumnName("Months_to_Goal");

                    b.Property<string>("ResultsJson")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("json_Results");

                    b.Property<int>("ScenarioId")
                        .HasColumnType("int")
                        .HasColumnName("Scenario_id");

                    b.HasKey("Id");

                    b.HasIndex("ScenarioId");

                    b.ToTable("Simulation_history", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.UserStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("userStatus", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("Roles_Identity", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Frontend.Models.Entities.Professor", b =>
                {
                    b.HasBaseType("Frontend.Models.Entities.ApplicationUser");

                    b.HasDiscriminator().HasValue("Professor");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Student", b =>
                {
                    b.HasBaseType("Frontend.Models.Entities.ApplicationUser");

                    b.Property<int?>("ClassroomId")
                        .HasColumnType("int");

                    b.HasIndex("ClassroomId");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Expense", b =>
                {
                    b.HasBaseType("Frontend.Models.Entities.FinancialEntry");

                    b.Property<bool>("IsEssential")
                        .HasColumnType("tinyint(1)");

                    b.HasDiscriminator().HasValue("Expense");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Income", b =>
                {
                    b.HasBaseType("Frontend.Models.Entities.FinancialEntry");

                    b.HasDiscriminator().HasValue("Income");
                });

            modelBuilder.Entity("Frontend.Models.Entities.ApplicationUser", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.RoleLookup", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.UserStatus", "UserStatus")
                        .WithMany()
                        .HasForeignKey("UserStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Gender");

                    b.Navigation("Role");

                    b.Navigation("UserStatus");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Challenge", b =>
                {
                    b.HasOne("Frontend.Models.Entities.ClassStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Classroom", b =>
                {
                    b.HasOne("Frontend.Models.Entities.ClassStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.Professor", "Teacher")
                        .WithMany("Classrooms")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Status");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Enrollment", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Classroom", "ClassGroup")
                        .WithMany("Enrollments")
                        .HasForeignKey("ClassGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassGroup");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Frontend.Models.Entities.FinancialEntry", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.EntryType", "EntryType")
                        .WithMany()
                        .HasForeignKey("EntryTypeId");

                    b.HasOne("Frontend.Models.Entities.Scenario", "Scenario")
                        .WithMany("Entries")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("EntryType");

                    b.Navigation("Scenario");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Objective", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Scenario", "Scenario")
                        .WithMany("Objectives")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scenario");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Scenario", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Student", "Student")
                        .WithMany("Scenarios")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Frontend.Models.Entities.SimulationHistory", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Scenario", "Scenario")
                        .WithMany("Histories")
                        .HasForeignKey("ScenarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scenario");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Frontend.Models.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Frontend.Models.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Frontend.Models.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Frontend.Models.Entities.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Frontend.Models.Entities.Student", b =>
                {
                    b.HasOne("Frontend.Models.Entities.Classroom", null)
                        .WithMany("Students")
                        .HasForeignKey("ClassroomId");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Classroom", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Scenario", b =>
                {
                    b.Navigation("Entries");

                    b.Navigation("Histories");

                    b.Navigation("Objectives");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Professor", b =>
                {
                    b.Navigation("Classrooms");
                });

            modelBuilder.Entity("Frontend.Models.Entities.Student", b =>
                {
                    b.Navigation("Enrollments");

                    b.Navigation("Scenarios");
                });
#pragma warning restore 612, 618
        }
    }
}
