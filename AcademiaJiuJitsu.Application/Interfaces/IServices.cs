using AcademiaJiuJitsu.Application.DTOs;

namespace AcademiaJiuJitsu.Application.Interfaces;

public interface IAlunoService
{
    Task<List<AlunoDto>> ListarAsync(string? busca = null);
    Task<AlunoDto?> ObterPorIdAsync(Guid id);
    Task<Guid> CriarAsync(AlunoDto dto);
    Task AtualizarAsync(AlunoDto dto);
    Task ExcluirAsync(Guid id);
    Task<List<AlunoInadimplenciaDto>> ListarInadimplenciaAsync(int diasAVencer = 7);
}

public interface IProfessorService
{
    Task<List<ProfessorDto>> ListarAsync();
    Task<Guid> SalvarAsync(ProfessorDto dto);
    Task ExcluirAsync(Guid id);
}

public interface IAulaService
{
    Task<List<AulaDto>> ListarAsync(Guid? professorId = null);
    Task<Guid> SalvarAsync(AulaDto dto);
    Task ExcluirAsync(Guid id);
}

public interface IFrequenciaService
{
    Task<Guid> RegistrarCheckInAsync(CheckInDto dto);
    Task<List<CheckInDto>> CheckInsDoDiaAsync(DateTime data);
    Task<List<CheckInDto>> RelatorioAsync(DateTime inicio, DateTime fim, Guid? alunoId = null);
}

public interface IFinanceiroService
{
    Task<Guid> SalvarLancamentoAsync(LancamentoFinanceiroDto dto);
    Task<List<LancamentoFinanceiroDto>> ListarLancamentosAsync(DateTime? inicio = null, DateTime? fim = null);
    Task<DashboardFinanceiroDto> ObterDashboardAsync();
}

public interface IPagamentoService
{
    DateTime CalcularVencimento(DateTime inicio, Domain.Enums.TipoPlano plano);
    Task<Guid> RegistrarPagamentoAsync(PagamentoAlunoDto dto);
    Task<List<PagamentoAlunoDto>> ListarPorAlunoAsync(Guid alunoId);
}
