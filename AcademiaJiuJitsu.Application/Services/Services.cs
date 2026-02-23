using AcademiaJiuJitsu.Application.DTOs;
using AcademiaJiuJitsu.Application.Interfaces;
using AcademiaJiuJitsu.Domain.Entities;
using AcademiaJiuJitsu.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AcademiaJiuJitsu.Application.Services;

public class AlunoService(IAppDbContext db) : IAlunoService
{
    public async Task<Guid> CriarAsync(AlunoDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Cpf) && await db.Alunos.AnyAsync(x => x.Cpf == dto.Cpf))
            throw new InvalidOperationException("CPF já cadastrado.");

        var aluno = new Aluno();
        Map(dto, aluno);
        db.Alunos.Add(aluno);
        await db.SaveChangesAsync();
        return aluno.Id;
    }

    public async Task AtualizarAsync(AlunoDto dto)
    {
        var aluno = await db.Alunos.FindAsync(dto.Id) ?? throw new KeyNotFoundException("Aluno não encontrado.");
        if (!string.IsNullOrWhiteSpace(dto.Cpf) && await db.Alunos.AnyAsync(x => x.Cpf == dto.Cpf && x.Id != dto.Id))
            throw new InvalidOperationException("CPF já cadastrado.");
        Map(dto, aluno);
        await db.SaveChangesAsync();
    }

    public async Task ExcluirAsync(Guid id)
    {
        var aluno = await db.Alunos.FindAsync(id);
        if (aluno is null) return;
        db.Alunos.Remove(aluno);
        await db.SaveChangesAsync();
    }

    public async Task<AlunoDto?> ObterPorIdAsync(Guid id)
        => await db.Alunos.Where(x => x.Id == id).Select(ToDto()).FirstOrDefaultAsync();

    public async Task<List<AlunoDto>> ListarAsync(string? busca = null)
    {
        var q = db.Alunos.AsQueryable();
        if (!string.IsNullOrWhiteSpace(busca))
            q = q.Where(x => x.NomeCompleto.Contains(busca) || x.Telefone.Contains(busca));
        return await q.OrderBy(x => x.NomeCompleto).Select(ToDto()).ToListAsync();
    }

    public async Task<List<AlunoInadimplenciaDto>> ListarInadimplenciaAsync(int diasAVencer = 7)
    {
        var limite = DateTime.Today.AddDays(diasAVencer);
        return await db.Alunos
            .Where(a => a.DataVencimentoAtual != null && a.DataVencimentoAtual <= limite)
            .Select(a => new AlunoInadimplenciaDto
            {
                AlunoId = a.Id,
                Nome = a.NomeCompleto,
                UltimoPagamento = a.DataUltimoPagamento,
                VencimentoAtual = a.DataVencimentoAtual,
                Situacao = a.DataVencimentoAtual < DateTime.Today ? SituacaoFinanceira.EmAtraso : SituacaoFinanceira.AVencer
            }).ToListAsync();
    }

    private static void Map(AlunoDto dto, Aluno aluno)
    {
        aluno.NomeCompleto = dto.NomeCompleto;
        aluno.Cpf = dto.Cpf;
        aluno.DataNascimento = dto.DataNascimento;
        aluno.Telefone = dto.Telefone;
        aluno.Email = dto.Email;
        aluno.Endereco = dto.Endereco;
        aluno.DataInicioMatricula = dto.DataInicioMatricula;
        aluno.Faixa = dto.Faixa;
        aluno.GrauAtual = dto.GrauAtual;
        aluno.DataUltimaGraduacao = dto.DataUltimaGraduacao;
        aluno.Observacoes = dto.Observacoes;
        aluno.Status = dto.Status;
        aluno.PlanoAtual = dto.PlanoAtual;
        aluno.ValorPlanoAtual = dto.ValorPlanoAtual;
        aluno.DataUltimoPagamento = dto.DataUltimoPagamento;
        aluno.DataVencimentoAtual = dto.DataVencimentoAtual;
    }

    private static System.Linq.Expressions.Expression<Func<Aluno, AlunoDto>> ToDto() => a => new AlunoDto
    {
        Id = a.Id,
        NomeCompleto = a.NomeCompleto,
        Cpf = a.Cpf,
        DataNascimento = a.DataNascimento,
        Telefone = a.Telefone,
        Email = a.Email,
        Endereco = a.Endereco,
        DataInicioMatricula = a.DataInicioMatricula,
        Faixa = a.Faixa,
        GrauAtual = a.GrauAtual,
        DataUltimaGraduacao = a.DataUltimaGraduacao,
        Observacoes = a.Observacoes,
        Status = a.Status,
        PlanoAtual = a.PlanoAtual,
        ValorPlanoAtual = a.ValorPlanoAtual,
        DataUltimoPagamento = a.DataUltimoPagamento,
        DataVencimentoAtual = a.DataVencimentoAtual
    };
}

public class ProfessorService(IAppDbContext db) : IProfessorService
{
    public async Task<List<ProfessorDto>> ListarAsync() => await db.Professores.Select(p => new ProfessorDto
    {
        Id = p.Id, Nome = p.Nome, Telefone = p.Telefone, Email = p.Email, Faixa = p.Faixa, DataInicio = p.DataInicio, Status = p.Status
    }).ToListAsync();

    public async Task<Guid> SalvarAsync(ProfessorDto dto)
    {
        Professor p;
        if (dto.Id == Guid.Empty)
        {
            p = new Professor();
            db.Professores.Add(p);
        }
        else p = await db.Professores.FindAsync(dto.Id) ?? throw new KeyNotFoundException();
        p.Nome = dto.Nome; p.Telefone = dto.Telefone; p.Email = dto.Email; p.Faixa = dto.Faixa; p.DataInicio = dto.DataInicio; p.Status = dto.Status;
        await db.SaveChangesAsync();
        return p.Id;
    }

    public async Task ExcluirAsync(Guid id)
    {
        var p = await db.Professores.FindAsync(id);
        if (p is null) return;
        db.Professores.Remove(p);
        await db.SaveChangesAsync();
    }
}

public class AulaService(IAppDbContext db) : IAulaService
{
    public async Task<List<AulaDto>> ListarAsync(Guid? professorId = null)
    {
        var q = db.AulasRecorrentes.Include(a => a.Professor).AsQueryable();
        if (professorId.HasValue) q = q.Where(a => a.ProfessorId == professorId);
        return await q.OrderBy(a => a.DiaSemana).ThenBy(a => a.HoraInicio).Select(a => new AulaDto
        {
            Id = a.Id,
            NomeModalidade = a.NomeModalidade,
            DiaSemana = a.DiaSemana,
            HoraInicio = a.HoraInicio,
            HoraFim = a.HoraFim,
            ProfessorId = a.ProfessorId,
            NomeProfessor = a.Professor!.Nome,
            Capacidade = a.Capacidade,
            Ativa = a.Ativa
        }).ToListAsync();
    }

    public async Task<Guid> SalvarAsync(AulaDto dto)
    {
        AulaRecorrente aula;
        if (dto.Id == Guid.Empty)
        {
            aula = new AulaRecorrente();
            db.AulasRecorrentes.Add(aula);
        }
        else aula = await db.AulasRecorrentes.FindAsync(dto.Id) ?? throw new KeyNotFoundException();

        aula.NomeModalidade = dto.NomeModalidade;
        aula.DiaSemana = dto.DiaSemana;
        aula.HoraInicio = dto.HoraInicio;
        aula.HoraFim = dto.HoraFim;
        aula.ProfessorId = dto.ProfessorId;
        aula.Capacidade = dto.Capacidade;
        aula.Ativa = dto.Ativa;
        await db.SaveChangesAsync();
        return aula.Id;
    }

    public async Task ExcluirAsync(Guid id)
    {
        var aula = await db.AulasRecorrentes.FindAsync(id);
        if (aula is null) return;
        db.AulasRecorrentes.Remove(aula);
        await db.SaveChangesAsync();
    }
}

public class FrequenciaService(IAppDbContext db) : IFrequenciaService
{
    public async Task<Guid> RegistrarCheckInAsync(CheckInDto dto)
    {
        var data = dto.DataHoraCheckIn.Date;
        var duplicado = await db.CheckIns.AnyAsync(c => c.AlunoId == dto.AlunoId && c.DataHoraCheckIn.Date == data &&
            ((dto.AulaRecorrenteId == null && c.AulaRecorrenteId == null) || c.AulaRecorrenteId == dto.AulaRecorrenteId));
        if (duplicado) throw new InvalidOperationException("Check-in duplicado para o mesmo dia/aula.");

        var check = new CheckIn
        {
            AlunoId = dto.AlunoId,
            AulaRecorrenteId = dto.AulaRecorrenteId,
            DataHoraCheckIn = dto.DataHoraCheckIn,
            Observacao = dto.Observacao
        };
        db.CheckIns.Add(check);
        await db.SaveChangesAsync();
        return check.Id;
    }

    public async Task<List<CheckInDto>> CheckInsDoDiaAsync(DateTime data)
        => await db.CheckIns.Include(c => c.Aluno).Where(c => c.DataHoraCheckIn.Date == data.Date)
            .OrderByDescending(c => c.DataHoraCheckIn).Select(c => new CheckInDto
            {
                Id = c.Id,
                AlunoId = c.AlunoId,
                AulaRecorrenteId = c.AulaRecorrenteId,
                DataHoraCheckIn = c.DataHoraCheckIn,
                Observacao = c.Observacao,
                NomeAluno = c.Aluno!.NomeCompleto
            }).ToListAsync();

    public async Task<List<CheckInDto>> RelatorioAsync(DateTime inicio, DateTime fim, Guid? alunoId = null)
    {
        var q = db.CheckIns.Include(c => c.Aluno).Where(c => c.DataHoraCheckIn.Date >= inicio.Date && c.DataHoraCheckIn.Date <= fim.Date);
        if (alunoId.HasValue) q = q.Where(c => c.AlunoId == alunoId);
        return await q.Select(c => new CheckInDto { Id = c.Id, AlunoId = c.AlunoId, NomeAluno = c.Aluno!.NomeCompleto, AulaRecorrenteId = c.AulaRecorrenteId, DataHoraCheckIn = c.DataHoraCheckIn, Observacao = c.Observacao }).ToListAsync();
    }
}

public class FinanceiroService(IAppDbContext db) : IFinanceiroService
{
    public async Task<Guid> SalvarLancamentoAsync(LancamentoFinanceiroDto dto)
    {
        var e = new LancamentoFinanceiro
        {
            Tipo = dto.Tipo,
            Categoria = dto.Categoria,
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Data = dto.Data,
            FormaPagamento = dto.FormaPagamento,
            AlunoId = dto.AlunoId
        };
        db.LancamentosFinanceiros.Add(e);
        await db.SaveChangesAsync();
        return e.Id;
    }

    public async Task<List<LancamentoFinanceiroDto>> ListarLancamentosAsync(DateTime? inicio = null, DateTime? fim = null)
    {
        var q = db.LancamentosFinanceiros.AsQueryable();
        if (inicio.HasValue) q = q.Where(x => x.Data >= inicio.Value.Date);
        if (fim.HasValue) q = q.Where(x => x.Data <= fim.Value.Date);
        return await q.OrderByDescending(x => x.Data).Select(x => new LancamentoFinanceiroDto
        {
            Id = x.Id, Tipo = x.Tipo, Categoria = x.Categoria, Descricao = x.Descricao, Valor = x.Valor, Data = x.Data, FormaPagamento = x.FormaPagamento, AlunoId = x.AlunoId
        }).ToListAsync();
    }

    public async Task<DashboardFinanceiroDto> ObterDashboardAsync()
    {
        var mesInicio = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var entradas = await db.LancamentosFinanceiros.Where(x => x.Tipo == TipoLancamento.Entrada).SumAsync(x => (decimal?)x.Valor) ?? 0;
        var saidas = await db.LancamentosFinanceiros.Where(x => x.Tipo == TipoLancamento.Saida).SumAsync(x => (decimal?)x.Valor) ?? 0;
        var entradasMes = await db.LancamentosFinanceiros.Where(x => x.Tipo == TipoLancamento.Entrada && x.Data >= mesInicio).SumAsync(x => (decimal?)x.Valor) ?? 0;
        var saidasMes = await db.LancamentosFinanceiros.Where(x => x.Tipo == TipoLancamento.Saida && x.Data >= mesInicio).SumAsync(x => (decimal?)x.Valor) ?? 0;
        var ultimos = await ListarLancamentosAsync(DateTime.Today.AddDays(-30), DateTime.Today);

        return new DashboardFinanceiroDto
        {
            SaldoAtual = entradas - saidas,
            EntradasMes = entradasMes,
            SaidasMes = saidasMes,
            UltimosLancamentos = ultimos.Take(10).ToList()
        };
    }
}

public class PagamentoService(IAppDbContext db) : IPagamentoService
{
    public DateTime CalcularVencimento(DateTime inicio, TipoPlano plano) => plano switch
    {
        TipoPlano.Mensal => inicio.AddMonths(1),
        TipoPlano.Trimestral => inicio.AddMonths(3),
        TipoPlano.Anual => inicio.AddMonths(12),
        _ => inicio.AddMonths(1)
    };

    public async Task<Guid> RegistrarPagamentoAsync(PagamentoAlunoDto dto)
    {
        var aluno = await db.Alunos.FindAsync(dto.AlunoId) ?? throw new KeyNotFoundException("Aluno não encontrado");
        if (dto.FimPeriodo == default) dto.FimPeriodo = CalcularVencimento(dto.InicioPeriodo, dto.Plano);

        var pag = new PagamentoAluno
        {
            AlunoId = dto.AlunoId,
            DataPagamento = dto.DataPagamento,
            InicioPeriodo = dto.InicioPeriodo,
            FimPeriodo = dto.FimPeriodo,
            Valor = dto.Valor,
            FormaPagamento = dto.FormaPagamento,
            Plano = dto.Plano,
            Observacao = dto.Observacao
        };
        db.PagamentosAlunos.Add(pag);

        aluno.DataUltimoPagamento = dto.DataPagamento;
        aluno.DataVencimentoAtual = dto.FimPeriodo;
        aluno.PlanoAtual = dto.Plano;
        aluno.ValorPlanoAtual = dto.Valor;

        db.LancamentosFinanceiros.Add(new LancamentoFinanceiro
        {
            Tipo = TipoLancamento.Entrada,
            Categoria = "Mensalidade",
            Descricao = $"Pagamento de {aluno.NomeCompleto}",
            Valor = dto.Valor,
            Data = dto.DataPagamento,
            FormaPagamento = dto.FormaPagamento,
            AlunoId = dto.AlunoId
        });

        await db.SaveChangesAsync();
        return pag.Id;
    }

    public async Task<List<PagamentoAlunoDto>> ListarPorAlunoAsync(Guid alunoId)
        => await db.PagamentosAlunos.Where(p => p.AlunoId == alunoId).OrderByDescending(p => p.DataPagamento)
            .Select(p => new PagamentoAlunoDto
            {
                Id = p.Id,
                AlunoId = p.AlunoId,
                DataPagamento = p.DataPagamento,
                InicioPeriodo = p.InicioPeriodo,
                FimPeriodo = p.FimPeriodo,
                Valor = p.Valor,
                FormaPagamento = p.FormaPagamento,
                Plano = p.Plano,
                Observacao = p.Observacao
            }).ToListAsync();
}
