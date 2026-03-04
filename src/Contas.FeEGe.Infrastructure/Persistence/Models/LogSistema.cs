/*
 * File documentation:
 * Representa registro de log técnico persistido no banco.
 * Existe para suporte a rastreabilidade em arquivo e base de dados.
 */
#region MaintenanceHistory
/*
 * File: LogSistema.cs
 * Purpose: Modelo inicial para tabela LOGSISTEMA.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial do modelo de log persistente.
 *
 * Notes:
 * - Nome da tabela física previsto: TLOGSISTEMA.
 */
#endregion

namespace Contas.FeEGe.Infrastructure.Persistence.Models;

public sealed class LogSistema
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Nivel { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string? Usuario { get; set; }
    public string? RequestId { get; set; }
    public string? Detalhes { get; set; }
}
