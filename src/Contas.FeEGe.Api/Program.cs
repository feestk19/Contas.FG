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

using Contas.FeEGe.Application.DependencyInjection;
using Contas.FeEGe.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
