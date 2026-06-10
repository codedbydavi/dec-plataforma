using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEC.Models
{
    public enum TipoTransacao
    {
        Rendimento,
        DespesaFixa,
        DespesaVariavel,
        Poupanca
    }

    public enum CategoriaTransacao
    {
        Salario,
        Subsidio,
        OutrosRendimentos,
        Habitacao,
        Alimentacao,
        Transporte,
        Saude,
        Educacao,
        Lazer,
        Seguros,
        Comunicacoes,
        Outros
    }

    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        [Required]
        public CategoriaTransacao Categoria { get; set; }

        [Required]
        public DateTime Data { get; set; } = DateTime.Now;

        public bool Recorrente { get; set; } = false;

        // Foreign Keys
        public int FamiliaId { get; set; }
        [ForeignKey("FamiliaId")]
        public virtual Familia? Familia { get; set; }
    }
}
