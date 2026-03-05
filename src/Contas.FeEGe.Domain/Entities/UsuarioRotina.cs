/*
 * File documentation:
 * Representa associacao entre usuario e rotina permitida.
 * Existe para persistir permissoes por rotina no modelo relacional.
 */
#region MaintenanceHistory
/*
 * File: UsuarioRotina.cs
 * Purpose: Entidade de relacionamento entre Usuario e Rotina.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial da entidade UsuarioRotina.
 *
 * Notes:
 * - Suporta atualizacao de permissoes por selecao de checkbox na UI.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class UsuarioRotina
{
    public long Id { get; set; }
    public long UsuarioId { get; set; }
    public long RotinaId { get; set; }
}
