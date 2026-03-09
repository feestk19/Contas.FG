/*
 * File documentation:
 * Expoe endpoints HTTP para operacoes de contas.
 * Existe para orquestrar entrada/saida da API no modulo financeiro principal.
 */
#region MaintenanceHistory
/*
 * File: ContasController.cs
 * Purpose: Endpoints de criacao e atualizacao de contas.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial do controller de contas.
 *
 * Notes:
 * - Regras de negocio permanecem centralizadas na camada Application.
 */
#endregion

using Contas.FeEGe.Api.Contracts;
using Contas.FeEGe.Application.Services;
using Contas.FeEGe.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Contas.FeEGe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ContasController : ControllerBase
{
    private readonly ContaService _contaService;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="ContasController"/>.
    /// </summary>
    /// <param name="contaService">Servico de aplicacao para regras de contas.</param>
    public ContasController(ContaService contaService)
    {
        _contaService = contaService;
    }

    /// <summary>
    /// Cria uma nova conta com base nos dados enviados na requisicao.
    /// </summary>
    /// <param name="request">Dados de entrada para criacao da conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da conta criada.</returns>
    [HttpPost]
    public async Task<IActionResult> CriarAsync([FromBody] CriarContaRequest request, CancellationToken cancellationToken)
    {
        var conta = new Conta
        {
            Descricao = request.Descricao,
            Valor = request.Valor,
            Tipo = request.Tipo,
            CategoriaId = request.CategoriaId,
            DataVencimento = request.DataVencimento,
            DataPagamento = request.DataPagamento,
            Status = request.Status,
            Observacao = request.Observacao
        };

        var id = await _contaService.CriarAsync(conta, cancellationToken);
        return Ok(new { id });
    }

    /// <summary>
    /// Marca uma conta como paga informando data de pagamento.
    /// </summary>
    /// <param name="id">Identificador da conta.</param>
    /// <param name="request">Dados do pagamento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPost("{id:long}/pagar")]
    public async Task<IActionResult> PagarAsync(long id, [FromBody] PagarContaRequest request, CancellationToken cancellationToken)
    {
        await _contaService.PagarAsync(id, request.DataPagamento, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Registra pagamento parcial e gera conta remanescente.
    /// </summary>
    /// <param name="id">Identificador da conta original.</param>
    /// <param name="request">Dados do pagamento parcial.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da nova conta remanescente.</returns>
    [HttpPost("{id:long}/pagamento-parcial")]
    public async Task<IActionResult> RegistrarPagamentoParcialAsync(long id, [FromBody] RegistrarPagamentoParcialRequest request, CancellationToken cancellationToken)
    {
        var novaContaId = await _contaService.RegistrarPagamentoParcialAsync(
            id,
            request.ValorPago,
            request.DataPagamento,
            request.DataProximoPagamento,
            cancellationToken);

        return Ok(new { id = novaContaId });
    }

    /// <summary>
    /// Reagenda o vencimento de uma conta com justificativa obrigatoria.
    /// </summary>
    /// <param name="id">Identificador da conta.</param>
    /// <param name="request">Dados para reagendamento da conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPost("{id:long}/reagendar")]
    public async Task<IActionResult> ReagendarAsync(long id, [FromBody] ReagendarContaRequest request, CancellationToken cancellationToken)
    {
        await _contaService.ReagendarAsync(
            id,
            request.NovaData,
            request.Motivo,
            request.Usuario,
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Cancela uma conta com justificativa obrigatoria.
    /// </summary>
    /// <param name="id">Identificador da conta.</param>
    /// <param name="request">Dados de cancelamento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Resposta de sucesso sem conteudo.</returns>
    [HttpPost("{id:long}/cancelar")]
    public async Task<IActionResult> CancelarAsync(long id, [FromBody] CancelarContaRequest request, CancellationToken cancellationToken)
    {
        await _contaService.CancelarAsync(id, request.Motivo, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Marca contas pendentes vencidas como em atraso para data de referencia informada.
    /// </summary>
    /// <param name="request">Payload opcional com data de referencia.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Total de contas atualizadas para atraso.</returns>
    [HttpPost("marcar-atraso")]
    public async Task<IActionResult> MarcarEmAtrasoAsync([FromBody] MarcarAtrasoRequest? request, CancellationToken cancellationToken)
    {
        var referencia = request?.Referencia ?? DateOnly.FromDateTime(DateTime.Today);
        var total = await _contaService.MarcarEmAtrasoAsync(referencia, cancellationToken);
        return Ok(new { total });
    }
}
