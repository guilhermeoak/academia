using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class Aluno : BaseEntity
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string? Cpf { get; set; }
    public DateTime? DataNascimento { get; set; }
    public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public DateTime DataInicioMatricula { get; set; } = DateTime.Today;
    public FaixaJiuJitsu Faixa { get; set; } = FaixaJiuJitsu.Branca;
    public int GrauAtual { get; set; }
    public DateTime? DataUltimaGraduacao { get; set; }
    public string? Observacoes { get; set; }
    public StatusCadastro Status { get; set; } = StatusCadastro.Ativo;

    public TipoPlano PlanoAtual { get; set; } = TipoPlano.Mensal;
    public decimal ValorPlanoAtual { get; set; }
    public DateTime? DataUltimoPagamento { get; set; }
    public DateTime? DataVencimentoAtual { get; set; }

    public ICollection<CheckIn> CheckIns { get; set; } = [];
    public ICollection<PagamentoAluno> Pagamentos { get; set; } = [];
    public ICollection<HistoricoGraduacao> HistoricoGraduacoes { get; set; } = [];
}
