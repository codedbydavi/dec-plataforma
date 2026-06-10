using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEC.Models
{
    public enum NivelDificuldade
    {
        Facil,
        Medio,
        Dificil
    }

    public class Desafio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public NivelDificuldade Nivel { get; set; }

        public int Pontos { get; set; }

        public string? ObjetivoAprendizagem { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativo { get; set; } = true;

        // Foreign Keys
        public int CriadoPorId { get; set; }
        [ForeignKey("CriadoPorId")]
        public virtual User? CriadoPor { get; set; }

        // Navigation properties
        public virtual ICollection<DesafioAluno>? DesafiosAluno { get; set; }
    }

    public class DesafioAluno
    {
        [Key]
        public int Id { get; set; }

        public int DesafioId { get; set; }
        [ForeignKey("DesafioId")]
        public virtual Desafio? Desafio { get; set; }

        public int AlunoId { get; set; }
        [ForeignKey("AlunoId")]
        public virtual User? Aluno { get; set; }

        public bool Completo { get; set; } = false;

        public DateTime? DataConclusao { get; set; }

        public int PontosGanhos { get; set; }
    }

    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        public string Icone { get; set; } = string.Empty;

        public int PontosNecessarios { get; set; }
    }
}
