/*
 * File documentation:
 * Registra dependencias da camada Infrastructure no container de DI.
 * Existe para centralizar bootstrap de persistencia e repositorios.
 */
#region MaintenanceHistory
/*
 * File: InfrastructureServiceCollectionExtensions.cs
 * Purpose: Extensoes de IServiceCollection para EF Core e repositorios.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial das configuracoes de infraestrutura.
 *
 * Notes:
 * - Usa SQL Server e ConnectionStrings:DefaultConnection.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Infrastructure.Persistence;
using Contas.FeEGe.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contas.FeEGe.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registra DbContext e repositorios da camada de infraestrutura.
    /// </summary>
    /// <param name="services">Colecao de servicos da aplicacao.</param>
    /// <param name="configuration">Configuracao para obter connection string.</param>
    /// <returns>A mesma colecao de servicos para encadeamento.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Server=localhost;Database=CONTASFEEGE;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<ContasFeEGeDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IContaRepository, ContaRepository>();
        services.AddScoped<IHistoricoReagendamentoRepository, HistoricoReagendamentoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ITipoContaRepository, TipoContaRepository>();
        services.AddScoped<IFonteRendaRepository, FonteRendaRepository>();
        services.AddScoped<IUsuarioPermissaoRepository, UsuarioPermissaoRepository>();

        return services;
    }
}
