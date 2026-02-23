using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class HistoricoGraduacao : BaseEntity
{
    public Guid AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
    public FaixaJiuJitsu Faixa { get; set; }
    public int Grau { get; set; }
    public DateTime DataGraduacao { get; set; } = DateTime.Today;
    public string? Observacao { get; set; }
}
