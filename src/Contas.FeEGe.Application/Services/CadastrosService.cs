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

    public CategoriaService(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

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

    public TipoContaService(ITipoContaRepository tipoContaRepository)
    {
        _tipoContaRepository = tipoContaRepository;
    }

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

    public FonteRendaService(IFonteRendaRepository fonteRendaRepository)
    {
        _fonteRendaRepository = fonteRendaRepository;
    }

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

    public UsuarioPermissaoService(IUsuarioPermissaoRepository usuarioPermissaoRepository)
    {
        _usuarioPermissaoRepository = usuarioPermissaoRepository;
    }

    public Task<bool> ValidarUsuarioNoExitAsync(string usuario, CancellationToken cancellationToken = default)
    {
        return _usuarioPermissaoRepository.UsuarioExisteAsync(usuario, cancellationToken);
    }

    public Task<IReadOnlyList<string>> ListarRotinasDisponiveisAsync(CancellationToken cancellationToken = default)
    {
        return _usuarioPermissaoRepository.ListarRotinasDisponiveisAsync(cancellationToken);
    }

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
