/*
 * File documentation:
 * Define contratos de entrada HTTP para endpoints da API.
 * Existe para separar payloads da camada de apresentacao dos modelos de dominio.
 */
#region MaintenanceHistory
/*
 * File: Requests.cs
 * Purpose: DTOs de requisicao para contas, cadastros e permissoes.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial dos contratos de requisicao da API.
 *
 * Notes:
 * - Mantem classes simples para facilitar serializacao JSON.
 */
#endregion

using Contas.FeEGe.Domain.Enums;

namespace Contas.FeEGe.Api.Contracts;

/// <summary>
/// Representa os dados para criacao de uma conta.
/// </summary>
public sealed class CriarContaRequest
{
    /// <summary>
    /// Descricao da conta para identificacao no sistema.
    /// </summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetario original da conta.
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Tipo do movimento financeiro da conta (Entrada = 1, Saida = 2).
    /// </summary>
    public TipoMovimento Tipo { get; set; }

    /// <summary>
    /// Identificador da categoria vinculada a conta.
    /// </summary>
    public long CategoriaId { get; set; }

    /// <summary>
    /// Data de vencimento da conta.
    /// </summary>
    public DateOnly DataVencimento { get; set; }

    /// <summary>
    /// Data de pagamento da conta quando ja houver quitacao.
    /// </summary>
    public DateOnly? DataPagamento { get; set; }

    /// <summary>
    /// Status inicial da conta (Pendente = 1, Pago = 2, EmAtraso = 3, Reagendado = 4, Cancelado = 5, PagoParcialmente = 6).
    /// </summary>
    public StatusConta Status { get; set; } = StatusConta.Pendente;

    /// <summary>
    /// Observacao opcional com detalhes adicionais da conta.
    /// </summary>
    public string? Observacao { get; set; }
}

/// <summary>
/// Representa os dados para registrar pagamento total de uma conta.
/// </summary>
public sealed class PagarContaRequest
{
    /// <summary>
    /// Data em que o pagamento foi realizado.
    /// </summary>
    public DateOnly DataPagamento { get; set; }
}

/// <summary>
/// Representa os dados para registrar pagamento parcial de uma conta.
/// </summary>
public sealed class RegistrarPagamentoParcialRequest
{
    /// <summary>
    /// Valor pago parcialmente na conta original.
    /// </summary>
    public decimal ValorPago { get; set; }

    /// <summary>
    /// Data do pagamento parcial.
    /// </summary>
    public DateOnly DataPagamento { get; set; }

    /// <summary>
    /// Nova data prevista para pagamento do saldo remanescente.
    /// </summary>
    public DateOnly DataProximoPagamento { get; set; }
}

/// <summary>
/// Representa os dados para reagendar o vencimento de uma conta.
/// </summary>
public sealed class ReagendarContaRequest
{
    /// <summary>
    /// Nova data de vencimento da conta.
    /// </summary>
    public DateOnly NovaData { get; set; }

    /// <summary>
    /// Justificativa obrigatoria para o reagendamento.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;

    /// <summary>
    /// Usuario responsavel pelo reagendamento.
    /// </summary>
    public string Usuario { get; set; } = string.Empty;
}

/// <summary>
/// Representa os dados para cancelamento de uma conta.
/// </summary>
public sealed class CancelarContaRequest
{
    /// <summary>
    /// Justificativa obrigatoria do cancelamento.
    /// </summary>
    public string Motivo { get; set; } = string.Empty;
}

/// <summary>
/// Representa os dados para marcacao de contas em atraso.
/// </summary>
public sealed class MarcarAtrasoRequest
{
    /// <summary>
    /// Data de referencia para considerar uma conta vencida. Quando nao informada, usa a data atual.
    /// </summary>
    public DateOnly? Referencia { get; set; }
}

/// <summary>
/// Representa os dados para criacao de uma categoria.
/// </summary>
public sealed class CriarCategoriaRequest
{
    /// <summary>
    /// Nome da categoria.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
}

/// <summary>
/// Representa os dados para criacao de um tipo de conta.
/// </summary>
public sealed class CriarTipoContaRequest
{
    /// <summary>
    /// Nome do tipo de conta.
    /// </summary>
    public string Nome { get; set; } = string.Empty;
}

/// <summary>
/// Representa os dados para criacao de uma fonte de renda.
/// </summary>
public sealed class CriarFonteRendaRequest
{
    /// <summary>
    /// Nome da fonte de renda.
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Valor mensal previsto para recebimento.
    /// </summary>
    public decimal ValorMensal { get; set; }

    /// <summary>
    /// Dia do mes previsto para recebimento da renda.
    /// </summary>
    public int DiaPrevistoRecebimento { get; set; }
}

/// <summary>
/// Representa os dados para associar rotinas a um usuario.
/// </summary>
public sealed class AssociarRotinasRequest
{
    /// <summary>
    /// Lista de codigos de rotina a serem associadas ao usuario.
    /// </summary>
    public IReadOnlyList<string> Rotinas { get; set; } = Array.Empty<string>();
}
