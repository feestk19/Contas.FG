/*
 * File documentation:
 * Expoe endpoints HTTP para cadastro de tipos de conta.
 * Existe para permitir gestao de tipos dinamicos na aplicacao.
 */
#region MaintenanceHistory
/*
 * File: TiposContaController.cs
 * Purpose: Endpoints de criacao e inativacao de tipos de conta.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do controller de tipos de conta.
 *
 * Notes:
 * - Mapeia requisicoes HTTP para rotinas do TipoContaService.
 */
#endregion

using Contas.FeEGe.Api.Contracts;
using Contas.FeEGe.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Controllers;

[ApiController]
[Route("api/tipos-conta")]
public sealed class TiposContaController : ControllerBase
{
    private readonly TipoContaService _tipoContaService;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="TiposContaController"/>.
    /// </summary>
    /// <param name="tipoContaService">Servico de aplicacao para tipos de conta.</param>
    public TiposContaController(TipoContaService tipoContaService)
    {
        _tipoContaService = tipoContaService;
    }

    /// <summary>
    /// Cria um novo tipo de conta.
    /// </summary>
    /// <param name="request">Dados do tipo de conta a ser criado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador do tipo de conta criado.</returns>
    [HttpPost]
    public async Task<IActionResult> CriarAsync([FromBody] CriarTipoContaRequest request, CancellationToken cancellationToken)
    {
        var id = await _tipoContaService.CriarAsync(request.Nome, cancellationToken);
        return Ok(new { id });
    }

    /// <summary>
    /// Inativa um tipo de conta existente.
    /// </summary>
    /// <param name="id">Identificador do tipo de conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPatch("{id:long}/inativar")]
    public async Task<IActionResult> InativarAsync(long id, CancellationToken cancellationToken)
    {
        await _tipoContaService.InativarAsync(id, cancellationToken);
        return NoContent();
    }
}
