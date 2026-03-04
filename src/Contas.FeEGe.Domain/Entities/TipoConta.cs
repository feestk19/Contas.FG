/*
 * File documentation:
 * Representa tipos configuráveis de contas financeiras.
 * Existe para permitir classificação dinâmica administrável pelo usuário.
 */
#region MaintenanceHistory
/*
 * File: TipoConta.cs
 * Purpose: Entidade de tipos de conta cadastráveis.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da entidade TipoConta.
 *
 * Notes:
 * - Soft delete por flag Ativo.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class TipoConta
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
