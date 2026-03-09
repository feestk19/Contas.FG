/*
 * File documentation:
 * Configura o pipeline HTTP e dependências de apresentação da API.
 * Existe para inicializar o host da aplicação e endpoints base.
 */
#region MaintenanceHistory
/*
 * File: Program.cs
 * Purpose: Bootstrap da camada Presentation (API).
 *
 * Maintenance History:
 * - 2026-03-04 | GitHub Copilot | Ajuste do bootstrap inicial com health checks.
 *
 * Notes:
 * - Estrutura inicial preparada para expansão de DI e autenticação JWT.
 */
#endregion

using System.Reflection;
using Contas.FeEGe.Application.DependencyInjection;
using Contas.FeEGe.Api.Extensions;
using Contas.FeEGe.Infrastructure.DependencyInjection;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Contas.FeEGe API",
        Version = "v1",
        Description = "API para gerenciamento de contas, cadastros e permissoes."
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.UseAllOfToExtendReferenceSchemas();
});
builder.Services.AddHealthChecks();

var app = builder.Build();

await app.InitializeDatabaseAsync();

if (!app.Environment.IsProduction())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "openapi/{documentName}.json";
    });

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Contas.FeEGe API v1");
        options.RoutePrefix = "swagger";
    });

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Contas.FeEGe API");
    });

    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
        .ExcludeFromDescription();
}

app.UseApiExceptionMapping();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
