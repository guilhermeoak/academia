using AcademiaJiuJitsu.Application.Interfaces;
using AcademiaJiuJitsu.Application.Services;
using AcademiaJiuJitsu.Infrastructure.Data;
using AcademiaJiuJitsu.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaJiuJitsu.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=academiajiujitsu.db";
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(conn));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IAlunoService, AlunoService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<IAulaService, AulaService>();
        services.AddScoped<IFrequenciaService, FrequenciaService>();
        services.AddScoped<IFinanceiroService, FinanceiroService>();
        services.AddScoped<IPagamentoService, PagamentoService>();

        return services;
    }
}
