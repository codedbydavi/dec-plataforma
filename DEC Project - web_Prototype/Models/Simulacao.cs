using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEC.Models
{
    public enum TipoSimulacao
    {
        JurosCompostos,
        AmortizacaoCredito,
        ProjecaoInflacao
    }

    public class Simulacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public TipoSimulacao Tipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorInicial { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxaJuros { get; set; }

        public int Periodo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorFinal { get; set; }

        public string? Parametros { get; set; } // JSON com parâmetros adicionais

        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Foreign Keys
        public int FamiliaId { get; set; }
        [ForeignKey("FamiliaId")]
        public virtual Familia? Familia { get; set; }
    }
}
