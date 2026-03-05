/*
 * File documentation:
 * Representa usuario autenticavel do sistema.
 * Existe para controlar acesso por perfil e permissoes de rotina.
 */
#region MaintenanceHistory
/*
 * File: Usuario.cs
 * Purpose: Entidade base de usuario para autenticacao e autorizacao.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial da entidade Usuario.
 *
 * Notes:
 * - ID em BIGINT para padrao de banco definido no projeto.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class Usuario
{
    public long Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
