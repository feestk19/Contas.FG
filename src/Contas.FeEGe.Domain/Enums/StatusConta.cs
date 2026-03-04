/*
 * File documentation:
 * Define os estados de ciclo de vida da conta no sistema.
 * Existe para suportar regras de transição e visualização por status.
 */
#region MaintenanceHistory
/*
 * File: StatusConta.cs
 * Purpose: Enum com status oficiais da entidade de conta.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial dos status de conta.
 *
 * Notes:
 * - Inclui PagoParcialmente conforme requisito funcional.
 */
#endregion

namespace Contas.FeEGe.Domain.Enums;

public enum StatusConta
{
    Pendente = 1,
    Pago = 2,
    EmAtraso = 3,
    Reagendado = 4,
    Cancelado = 5,
    PagoParcialmente = 6
}
