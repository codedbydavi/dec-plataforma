using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEC.Models
{
    public class Familia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        public int NumeroAdultos { get; set; } = 2;

        [Required]
        public int NumeroCriancas { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RendimentoMensal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DespesasFixas { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DespesasVariaveis { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Poupanca { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxaEsforco { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public bool Ativa { get; set; } = true;

        // Foreign Keys
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        // Navigation properties
        public virtual ICollection<Transacao>? Transacoes { get; set; }
        public virtual ICollection<Simulacao>? Simulacoes { get; set; }
    }
}
