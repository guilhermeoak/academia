using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class PagamentoAluno : BaseEntity
{
    public Guid AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
    public DateTime DataPagamento { get; set; } = DateTime.Today;
    public DateTime InicioPeriodo { get; set; }
    public DateTime FimPeriodo { get; set; }
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public TipoPlano Plano { get; set; }
    public string? Observacao { get; set; }
}
