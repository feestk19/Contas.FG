# Coding Standards (Obrigatório)

## 0) Regra de ouro
Antes de gerar qualquer código, **pergunte a stack** e confirme decisões técnicas mínimas:
- Tipo de app (MVC / API / Worker / etc.)
- Versão do .NET
- Banco de dados
- ORM (EF Core / Dapper / ambos)
- Autenticação (JWT / OIDC / etc.)
- Arquitetura (Clean / Layered / Minimal)

Se eu não responder, proponha 2-3 opções e peça pra eu escolher.

---

## 1) Cabeçalho padrão em TODO arquivo (Documentação + Histórico)
Todo arquivo C# deve começar com:

1) **Comentário de documentação do arquivo** (o que é e por que existe)
2) **Histórico de manutenção** em uma `#region` **logo acima dos imports**
3) Só depois: `using ...`

### Template obrigatório

#region MaintenanceHistory
/*
 * File: <NomeDoArquivo>.cs
 * Purpose: <1-2 linhas explicando o papel do arquivo>
 *
 * Maintenance History:
 * - YYYY-MM-DD | <Autor> | <Mudança objetiva>
 * - YYYY-MM-DD | <Autor> | <Mudança objetiva>
 *
 * Notes:
 * - <decisões importantes, riscos, dependências>
 */
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

> Observação: se o projeto usar `global using`, ainda assim o cabeçalho e o `#region` devem existir.

---

## 2) SOLID + DI (sem chororô)
- Sempre usar **Injeção de Dependência**
- Evitar `new` de serviços dentro de Controllers/Handlers/Services (exceto value objects simples)
- Preferir **interfaces** para dependências externas (repos, http clients, clocks, message buses)
- Cada classe com uma responsabilidade clara (SRP)
- Aberto para extensão, fechado para modificação (OCP) quando fizer sentido
- Evitar acoplamento desnecessário (DIP)

---

## 3) Design Patterns (QUANDO aplicável)
Use patterns **apenas quando resolverem um problema real**.
Quando aplicar, justifique em 1-2 linhas no cabeçalho (Notes).

Padrões comuns que podem aparecer:
- **Factory**: criação complexa/condicional
- **Strategy**: variações de regra por cenário
- **Decorator**: adicionar comportamento (logging, retry, cache) sem entupir a classe
- **Repository**: acesso a dados consistente
- **Unit of Work**: quando realmente precisar orquestrar transações (não por hábito)
- **Specification**: regras de consulta reutilizáveis
- **Adapter**: integração com serviços externos

> Regra: se o pattern só serve pra “parecer bonito”, NÃO use.

---

## 4) Organização de código
- Controllers: finos, sem regra de negócio
- Application/Services/Handlers: fluxo do caso de uso
- Domain: regras e invariantes do negócio
- Infrastructure: EF/SQL, integrações, providers

---

## 5) Convenções de nomenclatura
- Classes / Métodos / Propriedades: **PascalCase**
- Campos privados: **_camelCase** (com underscore)
- Interfaces: **I** + PascalCase (IUserService)
- Async: sufixo **Async** (GetByIdAsync)

---

## 6) Validação, erros e resultados
- Validação com **FluentValidation** (quando escolhido)
- Não usar exceção para fluxo normal
- Erros devem ser:
  - claros
  - logáveis
  - com correlation/request id quando houver

---

## 7) Qualidade mínima obrigatória
- Logging estruturado (quando definido na stack)
- Testes:
  - Unitários para regras
  - Integração para DB/externos (quando houver)
- Sem duplicação grosseira (DRY com bom senso)

---

## 8) Perguntas obrigatórias (sempre que necessário)
Se faltar contexto, pergunte antes de decidir:
- “Isso precisa ser transacional?”
- “Tem concorrência/consistência?”
- “Qual expectativa de volume?”
- “É multi-tenant?”
- “Quais integrações existem?”