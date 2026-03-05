/*
 * File documentation:
 * Implementa regras de negócio de contas a pagar e receber.
 * Existe para centralizar validações e transições de status do domínio financeiro.
 */
#region MaintenanceHistory
/*
 * File: ContaService.cs
 * Purpose: Serviço de aplicação para criação, pagamento, parcial, reagendamento e cancelamento.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial das rotinas principais de Conta.
 *
 * Notes:
 * - Mantém comportamento de pagamento futuro permitido.
 * - Mantém geração de nova conta no pagamento parcial.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Domain.Enums;

namespace Contas.FeEGe.Application.Services;

public sealed class ContaService
{
    private readonly IContaRepository _contaRepository;
    private readonly IHistoricoReagendamentoRepository _historicoReagendamentoRepository;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="ContaService"/>.
    /// </summary>
    /// <param name="contaRepository">Repositorio de contas.</param>
    /// <param name="historicoReagendamentoRepository">Repositorio de historico de reagendamento.</param>
    public ContaService(
        IContaRepository contaRepository,
        IHistoricoReagendamentoRepository historicoReagendamentoRepository)
    {
        _contaRepository = contaRepository;
        _historicoReagendamentoRepository = historicoReagendamentoRepository;
    }

    /// <summary>
    /// Cria uma nova conta aplicando validacoes de negocio.
    /// </summary>
    /// <param name="conta">Conta a ser criada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da conta criada.</returns>
    /// <exception cref="ArgumentException">Lancada quando houver dados invalidos para criacao.</exception>
    public async Task<long> CriarAsync(Conta conta, CancellationToken cancellationToken = default)
    {
        if (conta.Valor < 0)
        {
            throw new ArgumentException("Valor não pode ser negativo.");
        }

        if (conta.Status == StatusConta.Pago && conta.DataPagamento is null)
        {
            throw new ArgumentException("Conta paga exige data de pagamento.");
        }

        await _contaRepository.AddAsync(conta, cancellationToken);
        return conta.Id;
    }

    /// <summary>
    /// Marca uma conta como paga informando a data de pagamento.
    /// </summary>
    /// <param name="contaId">Identificador da conta.</param>
    /// <param name="dataPagamento">Data de pagamento registrada.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="KeyNotFoundException">Lancada quando a conta nao for encontrada.</exception>
    public async Task PagarAsync(long contaId, DateOnly dataPagamento, CancellationToken cancellationToken = default)
    {
        var conta = await _contaRepository.GetByIdAsync(contaId, cancellationToken)
            ?? throw new KeyNotFoundException("Conta não encontrada.");

        conta.DataPagamento = dataPagamento;
        conta.Status = StatusConta.Pago;

        await _contaRepository.UpdateAsync(conta, cancellationToken);
    }

    /// <summary>
    /// Registra pagamento parcial e cria uma nova conta para o valor remanescente.
    /// </summary>
    /// <param name="contaId">Identificador da conta original.</param>
    /// <param name="valorPago">Valor parcial pago.</param>
    /// <param name="dataPagamento">Data do pagamento parcial.</param>
    /// <param name="dataProximoPagamento">Data prevista para o proximo pagamento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da nova conta remanescente.</returns>
    /// <exception cref="KeyNotFoundException">Lancada quando a conta nao for encontrada.</exception>
    /// <exception cref="ArgumentException">Lancada quando o valor parcial for invalido.</exception>
    public async Task<long> RegistrarPagamentoParcialAsync(
        long contaId,
        decimal valorPago,
        DateOnly dataPagamento,
        DateOnly dataProximoPagamento,
        CancellationToken cancellationToken = default)
    {
        var conta = await _contaRepository.GetByIdAsync(contaId, cancellationToken)
            ?? throw new KeyNotFoundException("Conta não encontrada.");

        if (valorPago <= 0 || valorPago >= conta.Valor)
        {
            throw new ArgumentException("Valor parcial inválido.");
        }

        conta.Status = StatusConta.PagoParcialmente;
        conta.DataPagamento = dataPagamento;
        conta.DataProximoPagamento = dataProximoPagamento;

        await _contaRepository.UpdateAsync(conta, cancellationToken);

        var novaConta = new Conta
        {
            Descricao = $"{conta.Descricao} (remanescente)",
            Valor = conta.Valor - valorPago,
            Tipo = conta.Tipo,
            CategoriaId = conta.CategoriaId,
            DataVencimento = dataProximoPagamento,
            Status = StatusConta.Pendente
        };

        await _contaRepository.AddAsync(novaConta, cancellationToken);
        return novaConta.Id;
    }

    /// <summary>
    /// Reagenda o vencimento de uma conta e registra historico da alteracao.
    /// </summary>
    /// <param name="contaId">Identificador da conta a reagendar.</param>
    /// <param name="novaData">Nova data de vencimento.</param>
    /// <param name="motivo">Justificativa do reagendamento.</param>
    /// <param name="usuario">Usuario responsavel pela acao.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="ArgumentException">Lancada quando a justificativa nao for informada.</exception>
    /// <exception cref="KeyNotFoundException">Lancada quando a conta nao for encontrada.</exception>
    public async Task ReagendarAsync(
        long contaId,
        DateOnly novaData,
        string motivo,
        string usuario,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new ArgumentException("Justificativa obrigatória.");
        }

        var conta = await _contaRepository.GetByIdAsync(contaId, cancellationToken)
            ?? throw new KeyNotFoundException("Conta não encontrada.");

        var historico = new HistoricoReagendamento
        {
            ContaId = conta.Id,
            DataAnterior = conta.DataVencimento,
            DataNova = novaData,
            Motivo = motivo,
            Usuario = usuario,
            DataHora = DateTime.UtcNow
        };

        conta.DataVencimento = novaData;
        conta.Status = StatusConta.Reagendado;

        await _contaRepository.UpdateAsync(conta, cancellationToken);
        await _historicoReagendamentoRepository.AddAsync(historico, cancellationToken);
    }

    /// <summary>
    /// Cancela uma conta exigindo justificativa.
    /// </summary>
    /// <param name="contaId">Identificador da conta.</param>
    /// <param name="motivo">Justificativa do cancelamento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="ArgumentException">Lancada quando a justificativa nao for informada.</exception>
    /// <exception cref="KeyNotFoundException">Lancada quando a conta nao for encontrada.</exception>
    public async Task CancelarAsync(long contaId, string motivo, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            throw new ArgumentException("Justificativa obrigatória.");
        }

        var conta = await _contaRepository.GetByIdAsync(contaId, cancellationToken)
            ?? throw new KeyNotFoundException("Conta não encontrada.");

        conta.Status = StatusConta.Cancelado;
        conta.Observacao = motivo;

        await _contaRepository.UpdateAsync(conta, cancellationToken);
    }

    /// <summary>
    /// Marca automaticamente contas pendentes vencidas como em atraso.
    /// </summary>
    /// <param name="referencia">Data de referencia para identificacao de atraso.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Quantidade de contas atualizadas para atraso.</returns>
    public async Task<int> MarcarEmAtrasoAsync(DateOnly referencia, CancellationToken cancellationToken = default)
    {
        var contas = await _contaRepository.GetPendentesVencidasAsync(referencia, cancellationToken);

        foreach (var conta in contas)
        {
            conta.Status = StatusConta.EmAtraso;
            await _contaRepository.UpdateAsync(conta, cancellationToken);
        }

        return contas.Count;
    }
}
