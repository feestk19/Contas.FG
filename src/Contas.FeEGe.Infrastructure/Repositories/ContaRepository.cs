/*
 * File documentation:
 * Implementa persistencia de Conta com Entity Framework Core.
 * Existe para materializar contratos da camada Application em SQL Server.
 */
#region MaintenanceHistory
/*
 * File: ContaRepository.cs
 * Purpose: Repositorio de Conta com consultas e comandos principais.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial do repositorio de Conta.
 *
 * Notes:
 * - Usa AsNoTracking em consultas somente leitura para melhor desempenho.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Domain.Enums;
using Contas.FeEGe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contas.FeEGe.Infrastructure.Repositories;

public sealed class ContaRepository : IContaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="ContaRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public ContaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adiciona uma conta e persiste alteracoes no banco.
    /// </summary>
    /// <param name="conta">Entidade de conta a adicionar.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task AddAsync(Conta conta, CancellationToken cancellationToken)
    {
        await _dbContext.Contas.AddAsync(conta, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza uma conta e persiste alteracoes no banco.
    /// </summary>
    /// <param name="conta">Entidade de conta com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task UpdateAsync(Conta conta, CancellationToken cancellationToken)
    {
        _dbContext.Contas.Update(conta);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Busca uma conta por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Conta encontrada ou nulo quando inexistente.</returns>
    public Task<Conta?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Contas.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <summary>
    /// Lista contas pendentes vencidas antes da data de referencia.
    /// </summary>
    /// <param name="referencia">Data de referencia para atraso.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Colecao de contas pendentes vencidas.</returns>
    public async Task<IReadOnlyList<Conta>> GetPendentesVencidasAsync(DateOnly referencia, CancellationToken cancellationToken)
    {
        return await _dbContext.Contas
            .AsNoTracking()
            .Where(x => x.Status == StatusConta.Pendente && x.DataVencimento < referencia)
            .ToListAsync(cancellationToken);
    }
}
