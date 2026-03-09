/*
 * File documentation:
 * Expoe endpoints HTTP para cadastro de categorias.
 * Existe para permitir gestao de categorias pela camada de apresentacao.
 */
#region MaintenanceHistory
/*
 * File: CategoriasController.cs
 * Purpose: Endpoints de criacao e inativacao de categorias.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do controller de categorias.
 *
 * Notes:
 * - Operacoes seguem regras de negocio do CategoriaService.
 */
#endregion

using Contas.FeEGe.Api.Contracts;
using Contas.FeEGe.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public sealed class CategoriasController : ControllerBase
{
    private readonly CategoriaService _categoriaService;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="CategoriasController"/>.
    /// </summary>
    /// <param name="categoriaService">Servico de aplicacao para categorias.</param>
    public CategoriasController(CategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="request">Dados da categoria a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da categoria criada.</returns>
    [HttpPost]
    public async Task<IActionResult> CriarAsync([FromBody] CriarCategoriaRequest request, CancellationToken cancellationToken)
    {
        var id = await _categoriaService.CriarAsync(request.Nome, cancellationToken);
        return Ok(new { id });
    }

    /// <summary>
    /// Inativa uma categoria existente.
    /// </summary>
    /// <param name="id">Identificador da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPatch("{id:long}/inativar")]
    public async Task<IActionResult> InativarAsync(long id, CancellationToken cancellationToken)
    {
        await _categoriaService.InativarAsync(id, cancellationToken);
        return NoContent();
    }
}
