/*
 * File documentation:
 * Representa o histórico de alterações de vencimento da conta.
 * Existe para rastreabilidade e auditoria de reagendamentos.
 */
#region MaintenanceHistory
/*
 * File: HistoricoReagendamento.cs
 * Purpose: Entidade de histórico de reagendamento de contas.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da entidade HistoricoReagendamento.
 *
 * Notes:
 * - Justificativa e usuário são obrigatórios pela regra de negócio.
 */
#endregion

namespace Contas.FeEGe.Domain.Entities;

public sealed class HistoricoReagendamento
{
    public long Id { get; set; }
    public long ContaId { get; set; }
    public DateOnly DataAnterior { get; set; }
    public DateOnly DataNova { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
}
