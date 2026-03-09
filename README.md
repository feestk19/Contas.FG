# Contas.FG

## Ambiente de homologacao com Docker

O projeto esta configurado para subir em homologacao com dois containers:

- `sqlserver`: banco de dados SQL Server
- `api`: aplicacao ASP.NET Core

### Pre-requisitos

- Docker Desktop com `docker compose`
- Docker Desktop em execucao (engine Linux ativo)

### Subir o ambiente

```powershell
docker compose up -d --build
```

### Verificar containers

```powershell
docker compose ps
```

### Ver logs da API

```powershell
docker compose logs -f api
```

### Endpoints

- API: `http://localhost:8080`
- Health Check: `http://localhost:8080/health`
- OpenAPI JSON: `http://localhost:8080/openapi/v1.json`
- Swagger UI: `http://localhost:8080/swagger`
- Scalar: `http://localhost:8080/scalar/v1`

### Derrubar o ambiente

```powershell
docker compose down
```

Para derrubar removendo volume do banco:

```powershell
docker compose down -v
```

### Variaveis de ambiente

As variaveis usadas no `docker-compose.yml` estao em `.env.example`:

- `MSSQL_SA_PASSWORD`
- `SQLSERVER_DATABASE`
- `SQLSERVER_PORT`
- `API_PORT`
- `JWT_KEY`