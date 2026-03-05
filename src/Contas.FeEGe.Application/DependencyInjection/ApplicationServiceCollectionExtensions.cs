/*
 * File documentation:
 * Registra servicos de aplicacao no container de DI.
 * Existe para centralizar o bootstrap da camada Application.
 */
#region MaintenanceHistory
/*
 * File: ApplicationServiceCollectionExtensions.cs
 * Purpose: Extensao de IServiceCollection para servicos de caso de uso.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial do bootstrap da camada Application.
 *
 * Notes:
 * - Mantem Program.cs limpo e com responsabilidades de orquestracao.
 */
#endregion

using Contas.FeEGe.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Contas.FeEGe.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Registra os servicos da camada de aplicacao no container de dependencia.
    /// </summary>
    /// <param name="services">Colecao de servicos da aplicacao.</param>
    /// <returns>A mesma colecao de servicos para encadeamento.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ContaService>();
        services.AddScoped<CategoriaService>();
        services.AddScoped<TipoContaService>();
        services.AddScoped<FonteRendaService>();
        services.AddScoped<UsuarioPermissaoService>();

        return services;
    }
}
