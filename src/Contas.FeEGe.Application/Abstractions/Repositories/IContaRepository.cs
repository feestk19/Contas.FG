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
    Task AddAsync(Conta conta, CancellationToken cancellationToken);
    Task UpdateAsync(Conta conta, CancellationToken cancellationToken);
    Task<Conta?> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Conta>> GetPendentesVencidasAsync(DateOnly referencia, CancellationToken cancellationToken);
}
