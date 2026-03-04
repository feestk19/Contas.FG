/*
 * File documentation:
 * Contém cenários unitários de serviços de cadastro e permissões.
 * Existe para garantir validações e chamadas de repositório por rotina.
 */
#region MaintenanceHistory
/*
 * File: CadastrosServiceTests.cs
 * Purpose: Testes unitários para Categoria, TipoConta, FonteRenda e Permissões.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial da suíte de testes de cadastros.
 *
 * Notes:
 * - Utiliza mocks para validar contratos de persistência.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Application.Services;
using Contas.FeEGe.Domain.Entities;
using Moq;

namespace Contas.FeEGe.UnitTests.Services;

public sealed class CadastrosServiceTests
{
    [Fact]
    public async Task Categoria_CriarAsync_DeveFalhar_QuandoNomeVazio()
    {
        var repository = new Mock<ICategoriaRepository>();
        var service = new CategoriaService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync(" "));
    }

    [Fact]
    public async Task Categoria_InativarAsync_DeveAtualizarFlagAtivo()
    {
        const long categoriaId = 3001;
        var categoria = new Categoria { Id = categoriaId, Nome = "Moradia", Ativo = true };
        var repository = new Mock<ICategoriaRepository>();
        repository.Setup(x => x.GetByIdAsync(categoriaId, It.IsAny<CancellationToken>())).ReturnsAsync(categoria);

        var service = new CategoriaService(repository.Object);
        await service.InativarAsync(categoriaId);

        Assert.False(categoria.Ativo);
        repository.Verify(x => x.UpdateAsync(categoria, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task TipoConta_CriarAsync_DeveFalhar_QuandoNomeVazio()
    {
        var repository = new Mock<ITipoContaRepository>();
        var service = new TipoContaService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync(string.Empty));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1000, 0)]
    [InlineData(1000, 32)]
    public async Task FonteRenda_CriarAsync_DeveFalhar_QuandoCamposInvalidos(decimal valor, int dia)
    {
        var repository = new Mock<IFonteRendaRepository>();
        var service = new FonteRendaService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.CriarAsync("Salário", valor, dia));
    }

    [Fact]
    public async Task UsuarioPermissao_ValidarUsuarioNoExitAsync_DeveConsultarRepositorio()
    {
        var repository = new Mock<IUsuarioPermissaoRepository>();
        repository.Setup(x => x.UsuarioExisteAsync("admin", It.IsAny<CancellationToken>())).ReturnsAsync(true);
        var service = new UsuarioPermissaoService(repository.Object);

        var existe = await service.ValidarUsuarioNoExitAsync("admin");

        Assert.True(existe);
    }

    [Fact]
    public async Task UsuarioPermissao_AssociarRotinasAsync_DeveFalhar_QuandoSemRotinas()
    {
        var repository = new Mock<IUsuarioPermissaoRepository>();
        var service = new UsuarioPermissaoService(repository.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.AssociarRotinasAsync("operador", Array.Empty<string>()));
    }

    [Fact]
    public async Task UsuarioPermissao_AssociarRotinasAsync_DevePersistirAssociacao()
    {
        var repository = new Mock<IUsuarioPermissaoRepository>();
        var service = new UsuarioPermissaoService(repository.Object);
        var rotinas = new List<string> { "CONTAS_EDITAR", "CATEGORIA_CRIAR" };

        await service.AssociarRotinasAsync("operador", rotinas);

        repository.Verify(
            x => x.AssociarRotinasAsync("operador", It.Is<IReadOnlyList<string>>(r => r.Count == 2), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
