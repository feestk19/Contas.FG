/*
 * File documentation:
 * Define o tipo de movimento financeiro de uma conta (entrada ou saída).
 * Existe para padronizar regras de cálculo e filtros no domínio.
 */
#region MaintenanceHistory
/*
 * File: TipoMovimento.cs
 * Purpose: Enum de classificação de movimentação financeira.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial do enum de tipo de movimento.
 *
 * Notes:
 * - Mantido no domínio para evitar dependência de camadas externas.
 */
#endregion

namespace Contas.FeEGe.Domain.Enums;

public enum TipoMovimento
{
    Entrada = 1,
    Saida = 2
}
