/*
 * File documentation:
 * Representa rotina funcional do sistema para controle de permissao.
 * Existe para associar usuarios a acoes autorizadas.
 */
#region MaintenanceHistory
/*
 * File: Rotina.cs
 * Purpose: Entidade de rotina permissivel para RBAC por acao.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial da entidade Rotina.
 *
 * Notes:
 * - Codigo e usado na UI de checkboxes de associacao de rotinas.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class Rotina
{
    public long Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
