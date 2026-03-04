/*
 * File documentation:
 * Agrupa contratos de persistência para cadastros auxiliares.
 * Existe para manter os serviços de aplicação desacoplados de infraestrutura.
 */
#region MaintenanceHistory
/*
 * File: ICadastroRepositories.cs
 * Purpose: Interfaces de repositório para Categoria, TipoConta, FonteRenda e permissões.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial das abstrações de cadastros.
 *
 * Notes:
 * - Mantido em um arquivo para simplificar bootstrap inicial.
 */
#endregion

using Contas.FeEGe.Domain.Entities;

namespace Contas.FeEGe.Application.Abstractions.Repositories;

public interface IHistoricoReagendamentoRepository
{
    Task AddAsync(HistoricoReagendamento historico, CancellationToken cancellationToken);
}

public interface ICategoriaRepository
{
    Task AddAsync(Categoria categoria, CancellationToken cancellationToken);
    Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken);
    Task<Categoria?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface ITipoContaRepository
{
    Task AddAsync(TipoConta tipoConta, CancellationToken cancellationToken);
    Task UpdateAsync(TipoConta tipoConta, CancellationToken cancellationToken);
    Task<TipoConta?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface IFonteRendaRepository
{
    Task AddAsync(FonteRenda fonteRenda, CancellationToken cancellationToken);
    Task UpdateAsync(FonteRenda fonteRenda, CancellationToken cancellationToken);
    Task<FonteRenda?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface IUsuarioPermissaoRepository
{
    Task<bool> UsuarioExisteAsync(string usuario, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> ListarRotinasDisponiveisAsync(CancellationToken cancellationToken);
    Task AssociarRotinasAsync(string usuario, IReadOnlyList<string> rotinas, CancellationToken cancellationToken);
}
