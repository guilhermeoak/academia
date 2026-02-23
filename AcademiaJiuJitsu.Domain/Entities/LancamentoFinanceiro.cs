using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class LancamentoFinanceiro : BaseEntity
{
    public TipoLancamento Tipo { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; } = DateTime.Today;
    public FormaPagamento FormaPagamento { get; set; }
    public Guid? AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
}
