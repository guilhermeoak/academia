using AcademiaJiuJitsu.Application.Interfaces;
using AcademiaJiuJitsu.Domain.Entities;
using AcademiaJiuJitsu.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AcademiaJiuJitsu.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options), IAppDbContext
{
    public DbSet<Aluno> Alunos => Set<Aluno>();
    public DbSet<Professor> Professores => Set<Professor>();
    public DbSet<AulaRecorrente> AulasRecorrentes => Set<AulaRecorrente>();
    public DbSet<CheckIn> CheckIns => Set<CheckIn>();
    public DbSet<LancamentoFinanceiro> LancamentosFinanceiros => Set<LancamentoFinanceiro>();
    public DbSet<PagamentoAluno> PagamentosAlunos => Set<PagamentoAluno>();
    public DbSet<HistoricoGraduacao> HistoricosGraduacao => Set<HistoricoGraduacao>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Aluno>().HasIndex(x => x.Cpf).IsUnique();
        builder.Entity<Aluno>().Property(x => x.Cpf).IsRequired(false);

        builder.Entity<CheckIn>().HasIndex(x => new { x.AlunoId, x.AulaRecorrenteId, x.DataHoraCheckIn });

        builder.Entity<AulaRecorrente>()
            .HasOne(x => x.Professor)
            .WithMany(x => x.Aulas)
            .HasForeignKey(x => x.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CheckIn>()
            .HasOne(x => x.Aluno)
            .WithMany(x => x.CheckIns)
            .HasForeignKey(x => x.AlunoId);

        builder.Entity<PagamentoAluno>()
            .HasOne(x => x.Aluno)
            .WithMany(x => x.Pagamentos)
            .HasForeignKey(x => x.AlunoId);

        builder.Entity<HistoricoGraduacao>()
            .HasOne(x => x.Aluno)
            .WithMany(x => x.HistoricoGraduacoes)
            .HasForeignKey(x => x.AlunoId);

        builder.Entity<LancamentoFinanceiro>().HasData(
            new LancamentoFinanceiro { Id = Guid.Parse("1c1455d8-b3ba-4f84-9d6b-f4c6568e5c40"), Tipo = Domain.Enums.TipoLancamento.Saida, Categoria = "Aluguel", Descricao = "Aluguel da academia", Valor = 1500, Data = new DateTime(2024, 1, 1), FormaPagamento = Domain.Enums.FormaPagamento.Pix },
            new LancamentoFinanceiro { Id = Guid.Parse("53d1b2f3-4a90-4c0f-9061-267e9dd98ec8"), Tipo = Domain.Enums.TipoLancamento.Entrada, Categoria = "Evento", Descricao = "Seminário", Valor = 2000, Data = new DateTime(2024, 1, 5), FormaPagamento = Domain.Enums.FormaPagamento.Dinheiro }
        );
    }
}
