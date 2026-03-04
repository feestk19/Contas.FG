/*
 * File documentation:
 * Representa fonte de renda mensal configurável.
 * Existe para composição de entradas recorrentes e indicadores de saldo.
 */
#region MaintenanceHistory
/*
 * File: FonteRenda.cs
 * Purpose: Entidade de fonte de renda com valor e dia previsto.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da entidade FonteRenda.
 *
 * Notes:
 * - Campos mínimos definidos no discovery funcional.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class FonteRenda
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal ValorMensal { get; set; }
    public int DiaPrevistoRecebimento { get; set; }
    public bool Ativo { get; set; } = true;
}
