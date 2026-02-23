namespace AcademiaJiuJitsu.Domain.Entities;

public class CheckIn : BaseEntity
{
    public Guid AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
    public Guid? AulaRecorrenteId { get; set; }
    public AulaRecorrente? AulaRecorrente { get; set; }
    public DateTime DataHoraCheckIn { get; set; } = DateTime.Now;
    public string? Observacao { get; set; }
}
