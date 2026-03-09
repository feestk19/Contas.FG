/*
 * File documentation:
 * Configura mapeamento de excecoes para respostas padronizadas da API.
 * Existe para transformar erros de negocio em status HTTP adequados.
 */
#region MaintenanceHistory
/*
 * File: ExceptionMappingExtensions.cs
 * Purpose: Extensao para middleware de tratamento global de excecoes.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do mapeamento global de excecoes.
 *
 * Notes:
 * - Mapeia ArgumentException para 400 e KeyNotFoundException para 404.
 */
#endregion

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Extensions;

public static class ExceptionMappingExtensions
{
    /// <summary>
    /// Habilita middleware de excecoes para retornar ProblemDetails padronizado.
    /// </summary>
    /// <param name="app">Builder da aplicacao HTTP.</param>
    /// <returns>Builder da aplicacao para encadeamento.</returns>
    public static IApplicationBuilder UseApiExceptionMapping(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = feature?.Error;

                var (statusCode, title) = ResolveStatus(exception);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = exception?.Message
                };

                await context.Response.WriteAsJsonAsync(problem);
            });
        });

        return app;
    }

    /// <summary>
    /// Resolve codigo HTTP e titulo do problema com base no tipo da excecao.
    /// </summary>
    /// <param name="exception">Excecao capturada no pipeline.</param>
    /// <returns>Tupla com codigo HTTP e titulo da resposta.</returns>
    private static (int StatusCode, string Title) ResolveStatus(Exception? exception)
    {
        return exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Requisicao invalida"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Recurso nao encontrado"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor")
        };
    }
}
