/*
 * File documentation:
 * Representa categoria de classificação financeira.
 * Existe para agrupamento e análise de contas por categoria.
 */
#region MaintenanceHistory
/*
 * File: Categoria.cs
 * Purpose: Entidade de categoria com status ativo/inativo.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da entidade Categoria.
 *
 * Notes:
 * - Soft delete é representado por Ativo = false.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class Categoria
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
