/*
 * File documentation:
 * Contém cenários unitários das regras de ContaService.
 * Existe para validar comportamento de negócio com isolamento via mocks.
 */
#region MaintenanceHistory
/*
 * File: ContaServiceTests.cs
 * Purpose: Testes unitários das rotinas principais de contas.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da suíte de testes de ContaService.
 *
 * Notes:
 * - Usa xUnit + Moq conforme padrão definido no projeto.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Application.Services;
using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Domain.Enums;
using Moq;

namespace Contas.FeEGe.UnitTests.Services;

public sealed class ContaServiceTests
{
    private readonly Mock<IContaRepository> _contaRepositoryMock = new();
    private readonly Mock<IHistoricoReagendamentoRepository> _historicoRepositoryMock = new();

    /// <summary>
    /// Garante que a criacao de conta falha quando o valor informado e negativo.
    /// </summary>
    /// <param name="valor">Valor de entrada para o teste de validacao.</param>
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(-1000)]
    public async Task CriarAsync_DeveLancarExcecao_QuandoValorNegativo(decimal valor)
    {
        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        var conta = new Conta
        {
            Descricao = "Conta de teste",
            Valor = valor,
            DataVencimento = new DateOnly(2026, 3, 31),
            Status = StatusConta.Pendente
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync(conta));
    }

    /// <summary>
    /// Garante que nao e permitido criar conta com status pago sem data de pagamento.
    /// </summary>
    [Fact]
    public async Task CriarAsync_DeveLancarExcecao_QuandoPagoSemDataPagamento()
    {
        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        var conta = new Conta
        {
            Descricao = "Conta paga inválida",
            Valor = 120,
            Status = StatusConta.Pago,
            DataPagamento = null,
            DataVencimento = new DateOnly(2026, 3, 10)
        };

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync(conta));
    }

    /// <summary>
    /// Garante que pagamento com data futura e aceito e atualiza status da conta.
    /// </summary>
    [Fact]
    public async Task PagarAsync_DevePermitirDataFuturaEAtualizarStatus()
    {
        const long contaId = 1001;
        var conta = new Conta { Id = contaId, Valor = 100, Status = StatusConta.Pendente };
        _contaRepositoryMock
            .Setup(x => x.GetByIdAsync(contaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        var dataFutura = new DateOnly(2026, 12, 1);

        await service.PagarAsync(contaId, dataFutura);

        Assert.Equal(StatusConta.Pago, conta.Status);
        Assert.Equal(dataFutura, conta.DataPagamento);
        _contaRepositoryMock.Verify(x => x.UpdateAsync(conta, It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Garante que pagamento parcial rejeita valores fora das regras de negocio.
    /// </summary>
    /// <param name="valorPago">Valor parcial testado.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(500)]
    public async Task RegistrarPagamentoParcialAsync_DeveFalhar_QuandoValorInvalido(decimal valorPago)
    {
        const long contaId = 1002;
        var conta = new Conta
        {
            Id = contaId,
            Valor = 500,
            Descricao = "Parcial",
            Tipo = TipoMovimento.Saida,
            CategoriaId = 2001,
            DataVencimento = new DateOnly(2026, 3, 10)
        };

        _contaRepositoryMock
            .Setup(x => x.GetByIdAsync(contaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.RegistrarPagamentoParcialAsync(
                contaId,
                valorPago,
                new DateOnly(2026, 3, 5),
                new DateOnly(2026, 4, 5)));
    }

    /// <summary>
    /// Garante que pagamento parcial gera conta remanescente e atualiza status da conta original.
    /// </summary>
    [Fact]
    public async Task RegistrarPagamentoParcialAsync_DeveGerarContaRemanescente()
    {
        const long contaId = 1003;
        var conta = new Conta
        {
            Id = contaId,
            Valor = 500,
            Descricao = "Parcial",
            Tipo = TipoMovimento.Saida,
            CategoriaId = 2002,
            DataVencimento = new DateOnly(2026, 3, 10)
        };

        _contaRepositoryMock
            .Setup(x => x.GetByIdAsync(contaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);

        await service.RegistrarPagamentoParcialAsync(
            contaId,
            200,
            new DateOnly(2026, 3, 5),
            new DateOnly(2026, 4, 5));

        Assert.Equal(StatusConta.PagoParcialmente, conta.Status);
        _contaRepositoryMock.Verify(x => x.UpdateAsync(conta, It.IsAny<CancellationToken>()), Times.Once);
        _contaRepositoryMock.Verify(
            x => x.AddAsync(It.Is<Conta>(c => c.Valor == 300 && c.Status == StatusConta.Pendente), It.IsAny<CancellationToken>()),
            Times.Once);
    }

            /// <summary>
            /// Garante que reagendamento exige justificativa obrigatoria.
            /// </summary>
    [Fact]
    public async Task ReagendarAsync_DeveExigirJustificativa()
    {
        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.ReagendarAsync(1004, new DateOnly(2026, 5, 10), " ", "admin"));
    }

    /// <summary>
    /// Garante que reagendamento atualiza a conta e grava historico.
    /// </summary>
    [Fact]
    public async Task ReagendarAsync_DeveAtualizarContaERegistrarHistorico()
    {
        const long contaId = 1005;
        var conta = new Conta { Id = contaId, DataVencimento = new DateOnly(2026, 3, 10), Valor = 100 };
        _contaRepositoryMock
            .Setup(x => x.GetByIdAsync(contaId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);

        await service.ReagendarAsync(contaId, new DateOnly(2026, 3, 20), "Motivo", "operador");

        Assert.Equal(StatusConta.Reagendado, conta.Status);
        _historicoRepositoryMock.Verify(
            x => x.AddAsync(It.Is<HistoricoReagendamento>(h => h.ContaId == contaId && h.Motivo == "Motivo"), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Garante que cancelamento exige justificativa obrigatoria.
    /// </summary>
    [Fact]
    public async Task CancelarAsync_DeveExigirJustificativa()
    {
        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        await Assert.ThrowsAsync<ArgumentException>(() => service.CancelarAsync(1006, string.Empty));
    }

    /// <summary>
    /// Garante que contas pendentes vencidas sao marcadas como em atraso.
    /// </summary>
    [Fact]
    public async Task MarcarEmAtrasoAsync_DeveAtualizarTodasPendentesVencidas()
    {
        var contas = new List<Conta>
        {
            new() { Id = 1007, Status = StatusConta.Pendente },
            new() { Id = 1008, Status = StatusConta.Pendente }
        };

        _contaRepositoryMock
            .Setup(x => x.GetPendentesVencidasAsync(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contas);

        var service = new ContaService(_contaRepositoryMock.Object, _historicoRepositoryMock.Object);
        var total = await service.MarcarEmAtrasoAsync(new DateOnly(2026, 3, 4));

        Assert.Equal(2, total);
        _contaRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Conta>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }
}
