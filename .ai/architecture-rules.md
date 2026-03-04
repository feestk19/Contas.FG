# Architecture Rules

## Arquitetura padrão

Clean Architecture.

Camadas:

- Presentation
- Application
- Domain
- Infrastructure

## Regras entre camadas

Presentation → Application  
Application → Domain  
Infrastructure → Application/Domain  

Domain nunca depende de outras camadas.

## Estrutura do repositório

/src
  /Api
  /Application
  /Domain
  /Infrastructure

/tests
  /UnitTests
  /IntegrationTests

## Banco de dados

Preferência:

- SQL Server
- Entity Framework Core

## Logging

Utilizar:

Serilog

Logs devem conter:

- request id
- usuário
- timestamp
- erro

## Observabilidade

Sempre incluir:

- Health Checks
- Structured Logging

## Segurança

- JWT Authentication
- Authorization por roles/policies
- Validação de entrada obrigatória

## Boas práticas adicionais

- DTOs para entrada e saída
- Repository Pattern
- Unit of Work quando necessário
- Configuração via appsettings + environment variables