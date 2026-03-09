/*
 * File documentation:
 * Define rotina de inicializacao de banco para subir ambiente com schema pronto.
 * Existe para preparar tabelas e dados iniciais sem acao manual apos start da API.
 */
#region MaintenanceHistory
/*
 * File: DatabaseInitializationExtensions.cs
 * Purpose: Extensao de bootstrap de banco e seed inicial.
 *
 * Maintenance History:
 * - 2026-03-09 | GitHub Copilot | Criacao inicial da rotina de inicializacao de banco.
 *
 * Notes:
 * - Usa EnsureCreated para homologacao rapida em ambiente Docker.
 */
#endregion

using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Contas.FeEGe.Api.Extensions;

public static class DatabaseInitializationExtensions
{
    /// <summary>
    /// Inicializa o banco com criacao de schema e seed basica para o ambiente em execucao.
    /// </summary>
    /// <param name="app">Aplicacao web que fornece o container de servicos.</param>
    /// <returns>Tarefa assincrona de inicializacao.</returns>
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseBootstrap");
        var dbContext = scope.ServiceProvider.GetRequiredService<ContasFeEGeDbContext>();
        var connectionString = dbContext.Database.GetConnectionString();

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string padrao nao foi encontrada para inicializacao do banco.");
        }

        await WaitForSqlServerAsync(connectionString, logger);
        await EnsureDatabaseExistsAsync(connectionString, logger);
        await dbContext.Database.EnsureCreatedAsync();
        await SeedDataAsync(dbContext);
    }

    /// <summary>
    /// Aguarda disponibilidade do servidor SQL com tentativas de conexao controladas.
    /// </summary>
    /// <param name="connectionString">Connection string base da aplicacao.</param>
    /// <param name="logger">Logger para registrar tentativas de conexao.</param>
    /// <returns>Tarefa assincrona de espera de conexao.</returns>
    private static async Task WaitForSqlServerAsync(string connectionString, ILogger logger)
    {
        const int maxTentativas = 20;
        var masterConnectionString = BuildMasterConnectionString(connectionString);

        for (var tentativa = 1; tentativa <= maxTentativas; tentativa++)
        {
            try
            {
                await using var connection = new SqlConnection(masterConnectionString);
                await connection.OpenAsync();

                logger.LogInformation("Conexao com servidor SQL estabelecida na tentativa {Tentativa}.", tentativa);
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Tentativa {Tentativa} de conexao com servidor SQL falhou.", tentativa);
            }

            await Task.Delay(TimeSpan.FromSeconds(2));
        }

        throw new InvalidOperationException("Nao foi possivel conectar ao servidor SQL apos varias tentativas.");
    }

    /// <summary>
    /// Garante que o banco configurado exista antes da abertura do DbContext no catalogo alvo.
    /// </summary>
    /// <param name="connectionString">Connection string base da aplicacao.</param>
    /// <param name="logger">Logger para registrar a verificacao de criacao.</param>
    /// <returns>Tarefa assincrona de verificacao e criacao do banco.</returns>
    private static async Task EnsureDatabaseExistsAsync(string connectionString, ILogger logger)
    {
        var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
        var databaseName = connectionBuilder.InitialCatalog;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            logger.LogInformation("Connection string sem database definido; criacao explicita nao sera executada.");
            return;
        }

        await using var connection = new SqlConnection(BuildMasterConnectionString(connectionString));
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = @"
IF DB_ID(@dbName) IS NULL
BEGIN
    DECLARE @createSql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@dbName);
    EXEC(@createSql);
END";
        command.Parameters.AddWithValue("@dbName", databaseName);

        await command.ExecuteNonQueryAsync();
        logger.LogInformation("Banco {DatabaseName} verificado com sucesso.", databaseName);
    }

    /// <summary>
    /// Gera connection string apontando para o catalogo master para operacoes de bootstrap.
    /// </summary>
    /// <param name="connectionString">Connection string de origem da aplicacao.</param>
    /// <returns>Connection string ajustada para o banco master.</returns>
    private static string BuildMasterConnectionString(string connectionString)
    {
        var connectionBuilder = new SqlConnectionStringBuilder(connectionString)
        {
            InitialCatalog = "master"
        };

        return connectionBuilder.ConnectionString;
    }

    /// <summary>
    /// Semeia dados minimos para habilitar fluxo inicial de uso da API.
    /// </summary>
    /// <param name="dbContext">Contexto EF Core para persistencia dos dados iniciais.</param>
    /// <returns>Tarefa assincrona de seed.</returns>
    private static async Task SeedDataAsync(ContasFeEGeDbContext dbContext)
    {
        if (!await dbContext.Usuarios.AnyAsync())
        {
            dbContext.Usuarios.Add(new Usuario
            {
                Login = "admin",
                Nome = "Administrador",
                Ativo = true
            });
        }

        if (!await dbContext.Rotinas.AnyAsync())
        {
            dbContext.Rotinas.AddRange(
                new Rotina { Codigo = "CONTAS_CRIAR", Descricao = "Criar contas", Ativo = true },
                new Rotina { Codigo = "CONTAS_EDITAR", Descricao = "Editar contas", Ativo = true },
                new Rotina { Codigo = "CATEGORIA_CRIAR", Descricao = "Criar categorias", Ativo = true },
                new Rotina { Codigo = "PARAMETROS_EDITAR", Descricao = "Editar parametros", Ativo = true });
        }

        await dbContext.SaveChangesAsync();
    }
}
