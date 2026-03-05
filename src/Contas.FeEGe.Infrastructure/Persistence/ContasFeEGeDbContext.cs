/*
 * File documentation:
 * Define contexto EF Core do sistema Contas.FeEGe.
 * Existe para mapear entidades para tabelas SQL Server com padrao T... em uppercase.
 */
#region MaintenanceHistory
/*
 * File: ContasFeEGeDbContext.cs
 * Purpose: DbContext com mapeamentos de tabelas e relacionamentos principais.
 *
 * Maintenance History:
 * - 2026-03-05 | GitHub Copilot | Criacao inicial do DbContext e mapeamentos.
 *
 * Notes:
 * - IDs mapeados como BIGINT identity.
 * - Tabelas seguem padrao T + nome em uppercase.
 */
#endregion

using Contas.FeEGe.Domain.Entities;
using Contas.FeEGe.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Contas.FeEGe.Infrastructure.Persistence;

public sealed class ContasFeEGeDbContext : DbContext
{
    public ContasFeEGeDbContext(DbContextOptions<ContasFeEGeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Conta> Contas => Set<Conta>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<TipoConta> TiposConta => Set<TipoConta>();
    public DbSet<FonteRenda> FontesRenda => Set<FonteRenda>();
    public DbSet<HistoricoReagendamento> HistoricosReagendamento => Set<HistoricoReagendamento>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Rotina> Rotinas => Set<Rotina>();
    public DbSet<UsuarioRotina> UsuariosRotinas => Set<UsuarioRotina>();
    public DbSet<LogSistema> LogsSistema => Set<LogSistema>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureConta(modelBuilder);
        ConfigureCategoria(modelBuilder);
        ConfigureTipoConta(modelBuilder);
        ConfigureFonteRenda(modelBuilder);
        ConfigureHistoricoReagendamento(modelBuilder);
        ConfigureUsuario(modelBuilder);
        ConfigureRotina(modelBuilder);
        ConfigureUsuarioRotina(modelBuilder);
        ConfigureLogSistema(modelBuilder);
    }

    private static void ConfigureConta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Conta>(entity =>
        {
            entity.ToTable("TCONTAS");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Tipo)
                .HasColumnName("TIPOMOVIMENTO")
                .HasConversion<int>();

            entity.Property(x => x.CategoriaId)
                .HasColumnName("IDCATEGORIA");

            entity.Property(x => x.DataVencimento)
                .HasColumnName("DATAVENCIMENTO");

            entity.Property(x => x.DataPagamento)
                .HasColumnName("DATAPAGAMENTO");

            entity.Property(x => x.DataProximoPagamento)
                .HasColumnName("DATAPROXIMOPAGAMENTO");

            entity.Property(x => x.Status)
                .HasColumnName("STATUSCONTA")
                .HasConversion<int>();

            entity.Property(x => x.Observacao)
                .HasColumnName("OBSERVACAO")
                .HasMaxLength(500);

            entity.HasOne<Categoria>()
                .WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureCategoria(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("TCATEGORIA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Ativo)
                .HasColumnName("ATIVO")
                .HasDefaultValue(true);
        });
    }

    private static void ConfigureTipoConta(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoConta>(entity =>
        {
            entity.ToTable("TTIPOCONTA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Ativo)
                .HasColumnName("ATIVO")
                .HasDefaultValue(true);
        });
    }

    private static void ConfigureFonteRenda(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FonteRenda>(entity =>
        {
            entity.ToTable("TFONTERENDA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.ValorMensal)
                .HasColumnName("VALORMENSAL")
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.DiaPrevistoRecebimento)
                .HasColumnName("DIAPREVISTORECEBIMENTO");

            entity.Property(x => x.Ativo)
                .HasColumnName("ATIVO")
                .HasDefaultValue(true);
        });
    }

    private static void ConfigureHistoricoReagendamento(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HistoricoReagendamento>(entity =>
        {
            entity.ToTable("THISTREAGENDAMENTO");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.ContaId)
                .HasColumnName("IDCONTA");

            entity.Property(x => x.DataAnterior)
                .HasColumnName("DATAANTERIOR");

            entity.Property(x => x.DataNova)
                .HasColumnName("DATANOVA");

            entity.Property(x => x.Motivo)
                .HasColumnName("MOTIVO")
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(x => x.Usuario)
                .HasColumnName("USUARIO")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.DataHora)
                .HasColumnName("DATAHORA");

            entity.HasOne<Conta>()
                .WithMany()
                .HasForeignKey(x => x.ContaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TUSUARIO");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Login)
                .HasColumnName("LOGIN")
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Ativo)
                .HasColumnName("ATIVO")
                .HasDefaultValue(true);

            entity.HasIndex(x => x.Login)
                .IsUnique();
        });
    }

    private static void ConfigureRotina(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rotina>(entity =>
        {
            entity.ToTable("TROTINA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Codigo)
                .HasColumnName("CODIGO")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(x => x.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Ativo)
                .HasColumnName("ATIVO")
                .HasDefaultValue(true);

            entity.HasIndex(x => x.Codigo)
                .IsUnique();
        });
    }

    private static void ConfigureUsuarioRotina(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioRotina>(entity =>
        {
            entity.ToTable("TUSUARIOROTINA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.UsuarioId)
                .HasColumnName("IDUSUARIO");

            entity.Property(x => x.RotinaId)
                .HasColumnName("IDROTINA");

            entity.HasIndex(x => new { x.UsuarioId, x.RotinaId })
                .IsUnique();

            entity.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne<Rotina>()
                .WithMany()
                .HasForeignKey(x => x.RotinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureLogSistema(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogSistema>(entity =>
        {
            entity.ToTable("TLOGSISTEMA");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.Timestamp)
                .HasColumnName("TIMESTAMP")
                .IsRequired();

            entity.Property(x => x.Nivel)
                .HasColumnName("NIVEL")
                .HasMaxLength(32)
                .IsRequired();

            entity.Property(x => x.Mensagem)
                .HasColumnName("MENSAGEM")
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(x => x.Usuario)
                .HasColumnName("USUARIO")
                .HasMaxLength(120);

            entity.Property(x => x.RequestId)
                .HasColumnName("REQUESTID")
                .HasMaxLength(120);

            entity.Property(x => x.Detalhes)
                .HasColumnName("DETALHES");
        });
    }
}
