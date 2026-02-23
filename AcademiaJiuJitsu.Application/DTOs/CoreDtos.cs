using System.ComponentModel.DataAnnotations;
using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Application.DTOs;

public class ProfessorDto
{
    public Guid Id { get; set; }
    [Required] public string Nome { get; set; } = string.Empty;
    [Required] public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public FaixaJiuJitsu Faixa { get; set; }
    public DateTime? DataInicio { get; set; }
    public StatusCadastro Status { get; set; } = StatusCadastro.Ativo;
}

public class AulaDto
{
    public Guid Id { get; set; }
    [Required] public string NomeModalidade { get; set; } = string.Empty;
    public DiaSemana DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim { get; set; }
    [Required] public Guid ProfessorId { get; set; }
    public string? NomeProfessor { get; set; }
    public int? Capacidade { get; set; }
    public bool Ativa { get; set; } = true;
}

public class CheckInDto
{
    public Guid Id { get; set; }
    [Required] public Guid AlunoId { get; set; }
    public Guid? AulaRecorrenteId { get; set; }
    public DateTime DataHoraCheckIn { get; set; } = DateTime.Now;
    public string? Observacao { get; set; }
    public string NomeAluno { get; set; } = string.Empty;
}

public class LancamentoFinanceiroDto
{
    public Guid Id { get; set; }
    public TipoLancamento Tipo { get; set; }
    [Required] public string Categoria { get; set; } = string.Empty;
    [Required] public string Descricao { get; set; } = string.Empty;
    [Range(0.01, 999999)] public decimal Valor { get; set; }
    public DateTime Data { get; set; } = DateTime.Today;
    public FormaPagamento FormaPagamento { get; set; }
    public Guid? AlunoId { get; set; }
}

public class PagamentoAlunoDto
{
    public Guid Id { get; set; }
    [Required] public Guid AlunoId { get; set; }
    public DateTime DataPagamento { get; set; } = DateTime.Today;
    public DateTime InicioPeriodo { get; set; }
    public DateTime FimPeriodo { get; set; }
    [Range(0.01, 999999)] public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public TipoPlano Plano { get; set; }
    public string? Observacao { get; set; }
}

public class DashboardFinanceiroDto
{
    public decimal SaldoAtual { get; set; }
    public decimal EntradasMes { get; set; }
    public decimal SaidasMes { get; set; }
    public List<LancamentoFinanceiroDto> UltimosLancamentos { get; set; } = [];
}
