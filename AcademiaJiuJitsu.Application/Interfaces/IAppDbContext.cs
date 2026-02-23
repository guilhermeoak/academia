using AcademiaJiuJitsu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AcademiaJiuJitsu.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Aluno> Alunos { get; }
    DbSet<Professor> Professores { get; }
    DbSet<AulaRecorrente> AulasRecorrentes { get; }
    DbSet<CheckIn> CheckIns { get; }
    DbSet<LancamentoFinanceiro> LancamentosFinanceiros { get; }
    DbSet<PagamentoAluno> PagamentosAlunos { get; }
    DbSet<HistoricoGraduacao> HistoricosGraduacao { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
