/*
 * File documentation:
 * Define contrato de persistência para contas.
 * Existe para desacoplar regras de negócio da tecnologia de dados.
 */
#region MaintenanceHistory
/*
 * File: IContaRepository.cs
 * Purpose: Interface de acesso a dados da entidade Conta.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da abstração de repositório.
 *
 * Notes:
 * - Usado pela camada Application para seguir DIP/SOLID.
 */
#endregion

using Contas.FeEGe.Domain.Entities;

namespace Contas.FeEGe.Application.Abstractions.Repositories;

public interface IContaRepository
{
    /// <summary>
    /// Adiciona uma nova conta na base de dados.
    /// </summary>
    /// <param name="conta">Entidade de conta a ser persistida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AddAsync(Conta conta, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza uma conta existente na base de dados.
    /// </summary>
    /// <param name="conta">Entidade de conta com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task UpdateAsync(Conta conta, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera uma conta pelo identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Conta encontrada ou nulo quando inexistente.</returns>
    Task<Conta?> GetByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Lista contas pendentes cujo vencimento seja anterior a data de referencia.
    /// </summary>
    /// <param name="referencia">Data de referencia para filtro de atraso.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Colecao somente leitura de contas pendentes vencidas.</returns>
    Task<IReadOnlyList<Conta>> GetPendentesVencidasAsync(DateOnly referencia, CancellationToken cancellationToken);
}
