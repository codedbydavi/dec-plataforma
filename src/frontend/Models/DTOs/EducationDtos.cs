using Frontend.Models.DTOs;

namespace Frontend.Models.DTOs
{
    public class TurmaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Professor_Name { get; set; } = string.Empty;
        public string Codigo_Adesao { get; set; } = string.Empty;
        public int Aluno_Count { get; set; }
        public DateTime Data_Criacao { get; set; }
        public int? Class_Status { get; set; }
    }

    public class AlunoDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class TurmaDetalhesDto : TurmaDto
    {
        public List<AlunoDto> Alunos { get; set; } = new();
    }
}
