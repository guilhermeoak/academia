using AcademiaJiuJitsu.Domain.Enums;

namespace AcademiaJiuJitsu.Domain.Entities;

public class AulaRecorrente : BaseEntity
{
    public string NomeModalidade { get; set; } = string.Empty;
    public DiaSemana DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFim { get; set; }
    public Guid ProfessorId { get; set; }
    public Professor? Professor { get; set; }
    public int? Capacidade { get; set; }
    public bool Ativa { get; set; } = true;

    public ICollection<CheckIn> CheckIns { get; set; } = [];
}
