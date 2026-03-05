/*
 * File documentation:
 * Implementa serviços de cadastro base e associação de permissões.
 * Existe para atender regras operacionais sem acoplar UI à persistência.
 */
#region MaintenanceHistory
/*
 * File: CadastrosService.cs
 * Purpose: Serviços de Categoria, TipoConta, FonteRenda e Permissões de usuário.
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Criação inicial dos serviços de cadastros.
 *
 * Notes:
 * - Segue validações mínimas definidas no discovery.
 */
#endregion

using Contas.FeEGe.Application.Abstractions.Repositories;
using Contas.FeEGe.Domain.Entities;

namespace Contas.FeEGe.Application.Services;

public sealed class CategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="CategoriaService"/>.
    /// </summary>
    /// <param name="categoriaRepository">Repositorio de categorias.</param>
    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    /// <summary>
    /// Cria uma nova categoria.
    /// </summary>
    /// <param name="nome">Nome da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da categoria criada.</returns>
    /// <exception cref="ArgumentException">Lancada quando o nome for invalido.</exception>
    public async Task<long> CriarAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome obrigatório.");
        }

        var categoria = new Categoria
        {
            Nome = nome.Trim(),
            Ativo = true
        };

        await _categoriaRepository.AddAsync(categoria, cancellationToken);
        return categoria.Id;
    }

    /// <summary>
    /// Inativa uma categoria existente.
    /// </summary>
    /// <param name="id">Identificador da categoria.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="KeyNotFoundException">Lancada quando a categoria nao for encontrada.</exception>
    public async Task InativarAsync(long id, CancellationToken cancellationToken = default)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Categoria não encontrada.");

        categoria.Ativo = false;
        await _categoriaRepository.UpdateAsync(categoria, cancellationToken);
    }
}

public sealed class TipoContaService
{
    private readonly ITipoContaRepository _tipoContaRepository;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="TipoContaService"/>.
    /// </summary>
    /// <param name="tipoContaRepository">Repositorio de tipos de conta.</param>
    public TipoContaService(ITipoContaRepository tipoContaRepository)
    {
        _tipoContaRepository = tipoContaRepository;
    }

    /// <summary>
    /// Cria um novo tipo de conta.
    /// </summary>
    /// <param name="nome">Nome do tipo de conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador do tipo de conta criado.</returns>
    /// <exception cref="ArgumentException">Lancada quando o nome for invalido.</exception>
    public async Task<long> CriarAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome obrigatório.");
        }

        var tipoConta = new TipoConta
        {
            Nome = nome.Trim(),
            Ativo = true
        };

        await _tipoContaRepository.AddAsync(tipoConta, cancellationToken);
        return tipoConta.Id;
    }

    /// <summary>
    /// Inativa um tipo de conta existente.
    /// </summary>
    /// <param name="id">Identificador do tipo de conta.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="KeyNotFoundException">Lancada quando o tipo de conta nao for encontrado.</exception>
    public async Task InativarAsync(long id, CancellationToken cancellationToken = default)
    {
        var tipoConta = await _tipoContaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Tipo de conta não encontrado.");

        tipoConta.Ativo = false;
        await _tipoContaRepository.UpdateAsync(tipoConta, cancellationToken);
    }
}

public sealed class FonteRendaService
{
    private readonly IFonteRendaRepository _fonteRendaRepository;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="FonteRendaService"/>.
    /// </summary>
    /// <param name="fonteRendaRepository">Repositorio de fontes de renda.</param>
    public FonteRendaService(IFonteRendaRepository fonteRendaRepository)
    {
        _fonteRendaRepository = fonteRendaRepository;
    }

    /// <summary>
    /// Cria uma nova fonte de renda.
    /// </summary>
    /// <param name="nome">Nome da fonte.</param>
    /// <param name="valorMensal">Valor mensal recebido.</param>
    /// <param name="diaPrevistoRecebimento">Dia previsto para recebimento.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Identificador da fonte de renda criada.</returns>
    /// <exception cref="ArgumentException">Lancada quando os parametros forem invalidos.</exception>
    public async Task<long> CriarAsync(
        string nome,
        decimal valorMensal,
        int diaPrevistoRecebimento,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome obrigatório.");
        }

        if (valorMensal <= 0)
        {
            throw new ArgumentException("Valor mensal inválido.");
        }

        if (diaPrevistoRecebimento is < 1 or > 31)
        {
            throw new ArgumentException("Dia previsto inválido.");
        }

        var fonteRenda = new FonteRenda
        {
            Nome = nome.Trim(),
            ValorMensal = valorMensal,
            DiaPrevistoRecebimento = diaPrevistoRecebimento,
            Ativo = true
        };

        await _fonteRendaRepository.AddAsync(fonteRenda, cancellationToken);
        return fonteRenda.Id;
    }

    /// <summary>
    /// Inativa uma fonte de renda existente.
    /// </summary>
    /// <param name="id">Identificador da fonte de renda.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="KeyNotFoundException">Lancada quando a fonte de renda nao for encontrada.</exception>
    public async Task InativarAsync(long id, CancellationToken cancellationToken = default)
    {
        var fonte = await _fonteRendaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Fonte de renda não encontrada.");

        fonte.Ativo = false;
        await _fonteRendaRepository.UpdateAsync(fonte, cancellationToken);
    }
}

public sealed class UsuarioPermissaoService
{
    private readonly IUsuarioPermissaoRepository _usuarioPermissaoRepository;

    /// <summary>
    /// Inicializa uma nova instancia de <see cref="UsuarioPermissaoService"/>.
    /// </summary>
    /// <param name="usuarioPermissaoRepository">Repositorio de usuarios e permissoes.</param>
    public UsuarioPermissaoService(IUsuarioPermissaoRepository usuarioPermissaoRepository)
    {
        _usuarioPermissaoRepository = usuarioPermissaoRepository;
    }

    /// <summary>
    /// Valida se um usuario existe e esta ativo durante o evento de saida do campo.
    /// </summary>
    /// <param name="usuario">Login do usuario informado.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Verdadeiro quando o usuario existe e esta ativo.</returns>
    public Task<bool> ValidarUsuarioNoExitAsync(string usuario, CancellationToken cancellationToken = default)
    {
        return _usuarioPermissaoRepository.UsuarioExisteAsync(usuario, cancellationToken);
    }

    /// <summary>
    /// Lista rotinas disponiveis para associacao de permissoes.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <returns>Lista de codigos de rotina ativos.</returns>
    public Task<IReadOnlyList<string>> ListarRotinasDisponiveisAsync(CancellationToken cancellationToken = default)
    {
        return _usuarioPermissaoRepository.ListarRotinasDisponiveisAsync(cancellationToken);
    }

    /// <summary>
    /// Associa um conjunto de rotinas a um usuario.
    /// </summary>
    /// <param name="usuario">Login do usuario alvo.</param>
    /// <param name="rotinas">Colecao de rotinas selecionadas.</param>
    /// <param name="cancellationToken">Token de cancelamento da operacao.</param>
    /// <exception cref="ArgumentException">Lancada quando dados obrigatorios nao forem informados.</exception>
    public async Task AssociarRotinasAsync(string usuario, IReadOnlyList<string> rotinas, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(usuario))
        {
            throw new ArgumentException("Usuário obrigatório.");
        }

        if (rotinas is null || rotinas.Count == 0)
        {
            throw new ArgumentException("Selecione ao menos uma rotina.");
        }

        await _usuarioPermissaoRepository.AssociarRotinasAsync(usuario.Trim(), rotinas, cancellationToken);
    }
}
