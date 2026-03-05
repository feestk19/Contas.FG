/*
 * File documentation:
 * Implementa persistencia de permissoes por rotina para usuarios.
 * Existe para atender validacao de usuario no exit e associacao de checkboxes.
 */
#region MaintenanceHistory
/*
 * File: UsuarioPermissaoRepository.cs
 * Purpose: Repositorio para consulta de usuario e associacao de rotinas.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial do repositorio de permissoes.
 *
 * Notes:
 * - Rotinas nao encontradas na base sao ignoradas no processo de associacao.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contas.FeEGe.Infrastructure.Repositories;

public sealed class UsuarioPermissaoRepository : IUsuarioPermissaoRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="UsuarioPermissaoRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public UsuarioPermissaoRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Verifica se um usuario existe e esta ativo.
    /// </summary>
    /// <param name="usuario">Login do usuario.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Verdadeiro quando o usuario existe e esta ativo.</returns>
    public Task<bool> UsuarioExisteAsync(string usuario, CancellationToken cancellationToken)
    {
        var login = usuario.Trim();

        return _dbContext.Usuarios
            .AsNoTracking()
            .AnyAsync(x => x.Ativo && x.Login == login, cancellationToken);
    }

    /// <summary>
    /// Lista os codigos de rotinas ativas disponiveis para associacao.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Colecao de codigos de rotina ativos.</returns>
    public async Task<IReadOnlyList<string>> ListarRotinasDisponiveisAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Rotinas
            .AsNoTracking()
            .Where(x => x.Ativo)
            .OrderBy(x => x.Codigo)
            .Select(x => x.Codigo)
            .ToListAsync(cancellationToken);
    }

            /// <summary>
            /// Substitui as associacoes de rotinas de um usuario pelas rotinas informadas.
            /// </summary>
            /// <param name="usuario">Login do usuario alvo.</param>
            /// <param name="rotinas">Lista de codigos de rotina selecionados.</param>
            /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
            /// <exception cref="KeyNotFoundException">Lancada quando o usuario nao for encontrado.</exception>
    public async Task AssociarRotinasAsync(string usuario, IReadOnlyList<string> rotinas, CancellationToken cancellationToken)
    {
        var login = usuario.Trim();

        var usuarioDb = await _dbContext.Usuarios
            .FirstOrDefaultAsync(x => x.Login == login && x.Ativo, cancellationToken)
            ?? throw new KeyNotFoundException("Usuario nao encontrado.");

        var rotinasDb = await _dbContext.Rotinas
            .Where(x => x.Ativo && rotinas.Contains(x.Codigo))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var atuais = await _dbContext.UsuariosRotinas
            .Where(x => x.UsuarioId == usuarioDb.Id)
            .ToListAsync(cancellationToken);

        _dbContext.UsuariosRotinas.RemoveRange(atuais);

        var novasAssociacoes = rotinasDb
            .Select(rotinaId => new UsuarioRotina
            {
                UsuarioId = usuarioDb.Id,
                RotinaId = rotinaId
            })
            .ToList();

        if (novasAssociacoes.Count > 0)
        {
            await _dbContext.UsuariosRotinas.AddRangeAsync(novasAssociacoes, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
