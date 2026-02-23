using System.ComponentModel.DataAnnotations;
using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Application.DTOs;

public class AlunoDto
{
    public Guid Id { get; set; }
    [Required] public string NomeCompleto { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateTime? DataNascimento { get; set; }
    [Required] public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    [Required] public DateTime DataInicioMatricula { get; set; } = DateTime.Today;
    public FaixaJiuJitsu Faixa { get; set; }
    [Range(0, 10)] public int GrauAtual { get; set; }
    public DateTime? DataUltimaGraduacao { get; set; }
    public string? Observacoes { get; set; }
    public StatusCadastro Status { get; set; } = StatusCadastro.Ativo;
    public TipoPlano PlanoAtual { get; set; } = TipoPlano.Mensal;
    [Range(0, 99999)] public decimal ValorPlanoAtual { get; set; }
    public DateTime? DataUltimoPagamento { get; set; }
    public DateTime? DataVencimentoAtual { get; set; }
    public SituacaoFinanceira SituacaoFinanceira =>
        !DataVencimentoAtual.HasValue || DataVencimentoAtual.Value.Date >= DateTime.Today ?
            (DataVencimentoAtual.HasValue && DataVencimentoAtual.Value.Date <= DateTime.Today.AddDays(7) ? SituacaoFinanceira.AVencer : SituacaoFinanceira.EmDia)
            : SituacaoFinanceira.EmAtraso;
}

public class AlunoInadimplenciaDto
{
    public Guid AlunoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime? UltimoPagamento { get; set; }
    public DateTime? VencimentoAtual { get; set; }
    public SituacaoFinanceira Situacao { get; set; }
}
