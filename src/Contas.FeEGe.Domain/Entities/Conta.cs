/*
 * File documentation:
 * Representa a conta financeira principal do sistema.
 * Existe para centralizar estado e dados necessários às regras de negócio.
 */
#region MaintenanceHistory
/*
 * File: Conta.cs
 * Purpose: Entidade de conta (pagar/receber) com campos de controle financeiro.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da entidade Conta.
 *
 * Notes:
 * - Mantém dados para pagamento parcial e reagendamento.
 */
#endregion

using Contas.FeEGe.Domain.Enums;

namespace Contas.FeEGe.Domain.Entities;

public sealed class Conta
{
    public long Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoMovimento Tipo { get; set; }
    public long CategoriaId { get; set; }
    public DateOnly DataVencimento { get; set; }
    public DateOnly? DataPagamento { get; set; }
    public DateOnly? DataProximoPagamento { get; set; }
    public StatusConta Status { get; set; } = StatusConta.Pendente;
    public string? Observacao { get; set; }
}
