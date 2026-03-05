# Project Scope Document

## 1. Visao Geral
O sistema **Contas.FeEGe** e uma aplicacao web para gestao financeira familiar, focada em controle de contas a pagar e receber, analise por periodo e operacao diaria com regras de negocio claras.

Objetivo principal:
- Gerenciar entrada e saida de contas.
- Permitir acompanhamento por periodo (semanal, mensal, anual).
- Exibir indicadores e alertas para apoiar tomada de decisao.

Escala inicial:
- Ate 100 usuarios.

## 2. Contexto do Negocio
O projeto resolve a necessidade de organizar contas familiares com previsibilidade de vencimentos, historico de alteracoes e visao consolidada de resultados.

Problemas que o sistema endereca:
- Falta de controle padronizado de vencimentos e pagamentos.
- Dificuldade para identificar contas em atraso e proximas do vencimento.
- Baixa visibilidade de comparativos por periodo e por categoria.
- Necessidade de rastreabilidade (auditoria e logs).

## 3. Usuarios do Sistema
Perfis previstos:
- **Admin**: parametrizacoes, gestao de usuarios, configuracao de email/SMTP, rotinas administrativas e backup.
- **Operador**: operacao de contas e cadastros conforme permissoes associadas.
- **Consultor**: acesso somente leitura e consulta.

Modelo de autorizacao:
- Permissoes por rotina.
- Associacao de rotinas por checkbox em tela simplificada.
- Validacao em tempo real no campo usuario (evento de exit): se usuario existir, listar rotinas disponiveis.

## 4. Escopo Funcional
Funcionalidades principais do MVP:
- Contas a Pagar.
- Contas a Receber.
- Cadastro de Categorias.
- Cadastro de Tipos de Conta (totalmente cadastraveis).
- Cadastro de Fontes de Renda.
- Dashboard com indicadores.
- Graficos de evolucao mensal.
- Filtros por periodo.
- Notificacoes por email.
- Alertas em modal obrigatorio no login.
- Exportacao de relatorios em PDF.

Cadastro de conta (campos):
- Descricao
- Valor
- Tipo (entrada/saida)
- Categoria
- Data de vencimento
- Data de pagamento
- Status
- Observacao

Comportamentos especiais:
- Parcelamento no cadastro: quantidade de parcelas (2..N), com N parametrizavel.
- Geracao de parcelas sob demanda.
- Pagamento parcial gera nova conta remanescente.

## 5. Regras de Negocio
Regras criticas:
- Nao permitir valor negativo.
- Conta com status Pago exige data de pagamento.
- Pagamento futuro e permitido.
- Conta vencida sem pagamento muda automaticamente para EmAtraso.
- Reagendamento exige justificativa.
- Cancelamento exige justificativa.
- Pagamento parcial exige data de pagamento e previsao do proximo pagamento.
- PagoParcialmente possui tratamento visual proprio (cor exclusiva).
- Reagendamento altera status e grava historico.
- Exclusao fisica bloqueada por padrao; liberada somente apos X dias (padrao 365, parametrizavel).
- Soft delete nas entidades principais.
- Deduplicacao de notificacao por email (nao reenviar no mesmo dia para a mesma conta).

Status de conta:
- Pendente
- Pago
- EmAtraso
- Reagendado
- Cancelado
- PagoParcialmente

Regra de data em fim de semana/feriado:
- Padrao: manter data original.
- Parametro opcional: mover para proximo dia util.

## 6. Arquitetura Tecnica
Arquitetura adotada:
- **Clean Architecture**
- Camadas: Presentation -> Application -> Domain <- Infrastructure

Diretrizes principais:
- Domain nao depende de outras camadas.
- Controllers finos e regras no Application/Domain.
- DTOs para entrada e saida.
- Repository Pattern para acesso a dados.
- Unit of Work quando necessario.
- Validacao obrigatoria de entrada.
- Logs estruturados com contexto.
- Health checks.

Convencoes tecnicas do projeto:
- IDs das entidades como **BIGINT** (tipo `long` no codigo).
- Tabelas em caixa alta com prefixo `T`.
- Exemplos: `TUSUARIO`, `TPARAMSIS`, `TCATEGORIA`, `TCONTAS`, `TLOGSISTEMA`.

## 7. Stack Tecnologica
| Camada | Tecnologia |
|------|------|
| Backend | ASP.NET Core (.NET 8) |
| Frontend | Razor/Blazor + Bootstrap |
| Banco de Dados | SQL Server |
| ORM | Entity Framework Core (planejado para persistencia) |
| Autenticacao | Login local + JWT |
| Autorizacao | Permissoes por rotina (RBAC por acao) |
| Testes | xUnit + Moq |
| Logs | Serilog (padrao de projeto) + tabela `TLOGSISTEMA` |
| CI/CD | GitHub Actions (`.github/workflows/ci.yml`) |
| Deploy | Pipeline de deploy pausado por decisao de custo |

## 8. Estrutura do Repositorio
```text
/src
  /Contas.FeEGe.Api
  /Contas.FeEGe.Application
  /Contas.FeEGe.Domain
  /Contas.FeEGe.Infrastructure

/tests
  /UnitTests
    /Contas.FeEGe.UnitTests
  /IntegrationTests
    /Contas.FeEGe.IntegrationTests

/docs
  /escopo-projeto.md
```

## 9. Estrategia de Qualidade
Qualidade funcional:
- Testes unitarios para regras de negocio.
- Testes de integracao para fluxos com infraestrutura.
- Pipeline CI executando restore, build e testes.

Qualidade tecnica:
- Logging estruturado com timestamp, usuario e request id quando disponivel.
- Tratamento consistente de erros e mensagens claras.
- Auditoria de acoes criticas (criar, editar, cancelar, reagendar, pagar, excluir logico).

Seguranca:
- Login local.
- JWT.
- Permissoes por rotina.
- Recuperacao de senha por email com token.
- Expiracao de sessao por inatividade (padrao 5 minutos, parametrizavel).

## 10. Plano de Implementacao
1. Consolidar contratos de dominio e DTOs de entrada/saida.
2. Implementar persistencia com SQL Server e EF Core, mapeando tabelas em padrao `T...` e IDs BIGINT.
3. Implementar autenticacao JWT, autorizacao por rotina e fluxo de recuperacao de senha.
4. Implementar cadastros base (Categoria, TipoConta, FonteRenda, Parametros).
5. Implementar modulo de contas (pagar/receber, parcial, reagendamento, cancelamento, atraso automatico).
6. Implementar notificacoes por email e modal obrigatorio no login.
7. Implementar dashboard e graficos (semanal, mensal, anual, por categoria, comparativo mensal).
8. Implementar relatorios PDF (por periodo, vencidas, por categoria, comparativo mensal).
9. Implementar auditoria e log persistente em `TLOGSISTEMA`.
10. Expandir testes (unitarios e integracao) cobrindo cenarios de borda e regressao.

---

## Observacoes de estado atual
- CI em funcionamento sem etapa de deploy (decisao para evitar custo).
- Estrutura base e testes iniciais ja criados no repositorio.
