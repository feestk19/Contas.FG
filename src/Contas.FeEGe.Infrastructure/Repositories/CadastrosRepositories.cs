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

    public HistoricoReagendamentoRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(HistoricoReagendamento historico, CancellationToken cancellationToken)
    {
        await _dbContext.HistoricosReagendamento.AddAsync(historico, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CategoriaRepository : ICategoriaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    public CategoriaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Categoria categoria, CancellationToken cancellationToken)
    {
        await _dbContext.Categorias.AddAsync(categoria, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Categoria categoria, CancellationToken cancellationToken)
    {
        _dbContext.Categorias.Update(categoria);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Categoria?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.Categorias.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

public sealed class TipoContaRepository : ITipoContaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    public TipoContaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TipoConta tipoConta, CancellationToken cancellationToken)
    {
        await _dbContext.TiposConta.AddAsync(tipoConta, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TipoConta tipoConta, CancellationToken cancellationToken)
    {
        _dbContext.TiposConta.Update(tipoConta);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<TipoConta?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.TiposConta.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

public sealed class FonteRendaRepository : IFonteRendaRepository
{
    private readonly ContasFeEGeDbContext _dbContext;

    public FonteRendaRepository(ContasFeEGeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(FonteRenda fonteRenda, CancellationToken cancellationToken)
    {
        await _dbContext.FontesRenda.AddAsync(fonteRenda, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FonteRenda fonteRenda, CancellationToken cancellationToken)
    {
        _dbContext.FontesRenda.Update(fonteRenda);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<FonteRenda?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _dbContext.FontesRenda.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
