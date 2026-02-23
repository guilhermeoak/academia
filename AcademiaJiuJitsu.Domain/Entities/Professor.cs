using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class Professor : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public FaixaJiuJitsu Faixa { get; set; }
    public DateTime? DataInicio { get; set; }
    public StatusCadastro Status { get; set; } = StatusCadastro.Ativo;

    public ICollection<AulaRecorrente> Aulas { get; set; } = [];
}
