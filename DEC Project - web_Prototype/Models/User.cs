using System.ComponentModel.DataAnnotations;

namespace DEC.Models
{
    public enum UserRole
    {
        Aluno,
        Professor,
        Administrador
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public string? Turma { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<Familia>? Familias { get; set; }
        public virtual ICollection<Desafio>? DesafiosCriados { get; set; }
        public virtual ICollection<DesafioAluno>? DesafiosAluno { get; set; }
    }
}
