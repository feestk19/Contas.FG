/*
 * File documentation:
 * Implementa repositorios de cadastros auxiliares e historico de reagendamento.
 * Existe para suportar os servicos de aplicacao com EF Core.
 */
#region MaintenanceHistory
/*
 * File: CadastrosRepositories.cs
 * Purpose: Repositorios de Categoria, TipoConta, FonteRenda e HistoricoReagendamento.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial dos repositorios de cadastro.
 *
 * Notes:
 * - Estrutura agrupada para reduzir boilerplate na camada Infrastructure.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Contas.FeEGe.Infrastructure.Repositories;

public sealed class HistoricoReagendamentoRepository : IHistoricoReagendamentoRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="HistoricoReagendamentoRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public HistoricoReagendamentoRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adiciona um historico de reagendamento e persiste alteracoes.
    /// </summary>
    /// <param name="historico">Historico de reagendamento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task AddAsync(HistoricoReagendamento historico, CancellationToken cancellationToken)
    {
        await _dbContext.HistoricosReagendamento.AddAsync(historico, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CategoriaRepository : ICategoriaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="CategoriaRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public CategoriaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adiciona uma categoria e persiste alteracoes.
    /// </summary>
    /// <param name="categoria">Categoria a adicionar.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task AddAsync(Categoria categoria, CancellationToken cancellationToken)
    {
        await _dbContext.Categorias.AddAsync(categoria, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza uma categoria e persiste alteracoes.
    /// </summary>
    /// <param name="categoria">Categoria com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken)
    {
        _dbContext.Categorias.Update(categoria);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Busca categoria por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Categoria encontrada ou nulo.</returns>
    public Task<Categoria?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

public sealed class TipoContaRepository : ITipoContaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="TipoContaRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public TipoContaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adiciona um tipo de conta e persiste alteracoes.
    /// </summary>
    /// <param name="tipoConta">Tipo de conta a adicionar.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task AddAsync(TipoConta tipoConta, CancellationToken cancellationToken)
    {
        await _dbContext.TiposConta.AddAsync(tipoConta, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza um tipo de conta e persiste alteracoes.
    /// </summary>
    /// <param name="tipoConta">Tipo de conta com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task UpdateAsync(TipoConta tipoConta, CancellationToken cancellationToken)
    {
        _dbContext.TiposConta.Update(tipoConta);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Busca tipo de conta por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT do tipo de conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Tipo de conta encontrado ou nulo.</returns>
    public Task<TipoConta?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.TiposConta.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

public sealed class FonteRendaRepository : IFonteRendaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="FonteRendaRepository"/>.
    /// </summary>
    /// <param name="dbContext">Contexto de banco da aplicacao.</param>
    public FonteRendaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Adiciona uma fonte de renda e persiste alteracoes.
    /// </summary>
    /// <param name="fonteRenda">Fonte de renda a adicionar.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task AddAsync(FonteRenda fonteRenda, CancellationToken cancellationToken)
    {
        await _dbContext.FontesRenda.AddAsync(fonteRenda, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Atualiza uma fonte de renda e persiste alteracoes.
    /// </summary>
    /// <param name="fonteRenda">Fonte de renda com dados atualizados.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    public async Task UpdateAsync(FonteRenda fonteRenda, CancellationToken cancellationToken)
    {
        _dbContext.FontesRenda.Update(fonteRenda);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Busca fonte de renda por identificador.
    /// </summary>
    /// <param name="id">Identificador BIGINT da fonte de renda.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Fonte de renda encontrada ou nulo.</returns>
    public Task<FonteRenda?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.FontesRenda.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
