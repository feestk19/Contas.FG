/*
 * File documentation:
 * Expoe endpoints HTTP para cadastro de fontes de renda.
 * Existe para manter a entrada financeira parametrizavel na aplicacao.
 */
#region MaintenanceHistory
/*
 * File: FontesRendaController.cs
 * Purpose: Endpoints de criacao e inativacao de fontes de renda.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do controller de fontes de renda.
 *
 * Notes:
 * - Operacoes seguem as validacoes aplicadas no FonteRendaService.
 */
#endregion

using Contas.FeEGe.Api.Contracts;
using Contas.FeEGe.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Controllers;

[ApiController]
[Route("api/fontes-renda")]
public sealed class FontesRendaController : ControllerBase
{
    private readonly FonteRendaService _fonteRendaService;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="FontesRendaController"/>.
    /// </summary>
    /// <param name="fonteRendaService">Servico de aplicacao para fontes de renda.</param>
    public FontesRendaController(FonteRendaService fonteRendaService)
    {
        _fonteRendaService = fonteRendaService;
    }

    /// <summary>
    /// Cria uma nova fonte de renda.
    /// </summary>
    /// <param name="request">Dados da fonte de renda a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da fonte de renda criada.</returns>
    [HttpPost]
    public async Task<IActionResult> CriarAsync([FromBody] CriarFonteRendaRequest request, CancellationToken cancellationToken)
    {
        var id = await _fonteRendaService.CriarAsync(
            request.Nome,
            request.ValorMensal,
            request.DiaPrevistoRecebimento,
            cancellationToken);

        return Ok(new { id });
    }

    /// <summary>
    /// Inativa uma fonte de renda existente.
    /// </summary>
    /// <param name="id">Identificador da fonte de renda.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPatch("{id:long}/inativar")]
    public async Task<IActionResult> InativarAsync(long id, CancellationToken cancellationToken)
    {
        await _fonteRendaService.InativarAsync(id, cancellationToken);
        return NoContent();
    }
}
