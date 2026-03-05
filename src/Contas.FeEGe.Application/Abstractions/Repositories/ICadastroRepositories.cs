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
    /// <summary>
    /// Persiste um registro de historico de reagendamento.
    /// </summary>
    /// <param name="historico">Historico de reagendamento a ser gravado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AddAsync(HistoricoReagendamento historico, CancellationToken cancellationToken);
}

public interface ICategoriaRepository
{
    /// <summary>
    /// Adiciona uma categoria na base de dados.
    /// </summary>
    /// <param name="categoria">Categoria a ser persistida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AddAsync(Categoria categoria, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza uma categoria existente.
    /// </summary>
    /// <param name="categoria">Categoria com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera categoria por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Categoria encontrada ou nulo.</returns>
    Task<Categoria?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface ITipoContaRepository
{
    /// <summary>
    /// Adiciona um tipo de conta na base de dados.
    /// </summary>
    /// <param name="tipoConta">Tipo de conta a ser persistido.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AddAsync(TipoConta tipoConta, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza um tipo de conta existente.
    /// </summary>
    /// <param name="tipoConta">Tipo de conta com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task UpdateAsync(TipoConta tipoConta, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera tipo de conta por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT do tipo de conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Tipo de conta encontrado ou nulo.</returns>
    Task<TipoConta?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface IFonteRendaRepository
{
    /// <summary>
    /// Adiciona uma fonte de renda na base de dados.
    /// </summary>
    /// <param name="fonteRenda">Fonte de renda a ser persistida.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AddAsync(FonteRenda fonteRenda, CancellationToken cancellationToken);

    /// <summary>
    /// Atualiza uma fonte de renda existente.
    /// </summary>
    /// <param name="fonteRenda">Fonte de renda com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task UpdateAsync(FonteRenda fonteRenda, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera fonte de renda por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da fonte de renda.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Fonte de renda encontrada ou nulo.</returns>
    Task<FonteRenda?> GetByIdAsync(long id, CancellationToken cancellationToken);
}

public interface IUsuarioPermissaoRepository
{
    /// <summary>
    /// Verifica se o usuario existe e esta ativo.
    /// </summary>
    /// <param name="usuario">Login do usuario.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Verdadeiro quando o usuario existe e esta ativo.</returns>
    Task<bool> UsuarioExisteAsync(string usuario, CancellationToken cancellationToken);

    /// <summary>
    /// Lista codigos de rotinas ativas disponiveis para associacao.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Colecao de codigos de rotina ordenada.</returns>
    Task<IReadOnlyList<string>> ListarRotinasDisponiveisAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Associa um conjunto de rotinas a um usuario.
    /// </summary>
    /// <param name="usuario">Login do usuario alvo.</param>
    /// <param name="rotinas">Lista de codigos de rotina selecionados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    Task AssociarRotinasAsync(string usuario, IReadOnlyList<string> rotinas, CancellationToken cancellationToken);
}
