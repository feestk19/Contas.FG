/*
 * File documentation:
 * Expoe endpoints HTTP para consulta e associacao de permissoes por rotina.
 * Existe para suportar o fluxo de validacao de usuario e checkboxes de rotinas.
 */
#region MaintenanceHistory
/*
 * File: UsuariosPermissoesController.cs
 * Purpose: Endpoints de validacao de usuario e associacao de rotinas.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do controller de usuarios/permissoes.
 *
 * Notes:
 * - Mantem regra de negocio na camada Application.
 */
#endregion

using Contas.FeEGe.Api.Contracts;
using Contas.FeEGe.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Controllers;

[ApiController]
[Route("api/usuarios-permissoes")]
public sealed class UsuariosPermissoesController : ControllerBase
{
    private readonly UsuarioPermissaoService _usuarioPermissaoService;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="UsuariosPermissoesController"/>.
    /// </summary>
    /// <param name="usuarioPermissaoService">Servico de aplicacao para permissoes de usuario.</param>
    public UsuariosPermissoesController(UsuarioPermissaoService usuarioPermissaoService)
    {
        _usuarioPermissaoService = usuarioPermissaoService;
    }

    /// <summary>
    /// Verifica se um usuario existe e esta ativo no sistema.
    /// </summary>
    /// <param name="usuario">Login do usuario para validacao.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Flag indicando existencia de usuario ativo.</returns>
    [HttpGet("{usuario}/existe")]
    public async Task<IActionResult> ValidarExistenciaAsync(string usuario, CancellationToken cancellationToken)
    {
        var existe = await _usuarioPermissaoService.ValidarUsuarioNoExitAsync(usuario, cancellationToken);
        return Ok(new { existe });
    }

    /// <summary>
    /// Lista codigos de rotinas ativas disponiveis para associacao.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Lista de codigos de rotina.</returns>
    [HttpGet("rotinas")]
    public async Task<IActionResult> ListarRotinasAsync(CancellationToken cancellationToken)
    {
        var rotinas = await _usuarioPermissaoService.ListarRotinasDisponiveisAsync(cancellationToken);
        return Ok(rotinas);
    }

    /// <summary>
    /// Associa um conjunto de rotinas a um usuario.
    /// </summary>
    /// <param name="usuario">Login do usuario alvo.</param>
    /// <param name="request">Rotinas selecionadas para associacao.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPost("{usuario}/rotinas")]
    public async Task<IActionResult> AssociarRotinasAsync(string usuario, [FromBody] AssociarRotinasRequest request, CancellationToken cancellationToken)
    {
        await _usuarioPermissaoService.AssociarRotinasAsync(usuario, request.Rotinas, cancellationToken);
        return NoContent();
    }
}
