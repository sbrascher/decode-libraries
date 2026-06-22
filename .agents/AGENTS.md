# Custom Agent Rules for Decode Libraries

Este arquivo define regras de desenvolvimento, diretrizes arquiteturais e preferências para agentes de IA que atuarem neste workspace.

## 🛠️ Tecnologias e Frameworks
- **Linguagem:** C# 12+ (aproveitando recursos modernos do C# como Primary Constructors, File-Scoped Namespaces, Nullable Reference Types, Collection Expressions e Pattern Matching).
- **Frameworks Alvo:** .NET Standard 2.1 (abstrações) e .NET 8 / .NET 9 / .NET 10 (implementações de alta performance).
- **Solução:** [Decode.sln](file:///D:/Projetos/Decode/Libraries/src/Decode.sln)

## 📏 Diretrizes de Código e Estilo
1. **Idioma:**
   - Todo o código (nomes de classes, métodos, variáveis, etc.), comentários XML (`/// <summary>`) e mensagens internas **DEVEM** ser escritos em **Inglês**.
   - Preserve sempre o idioma e o tom técnico.
2. **Qualidade de Código:**
   - Nullable Reference Types (`#nullable enable`) devem estar habilitados e respeitados (evite avisos/warnings do compilador).
   - Use File-scoped Namespaces (`namespace MeuNamespace;`) em vez do estilo em bloco tradicional.
   - Escreva comentários XML detalhados em todos os membros e tipos públicos.
   - **Tipagem Explícita:** Prefira tipagem explícita em vez do uso do tipo implícito `var` (ex: use `string token = ...`, `using Activity? activity = ...`, `foreach (DomainNotification notification in ...)`). Evite o uso de `var` a menos que seja estritamente necessário (como para tipos anônimos).
3. **Padrões de Projeto (Design Patterns):**
   - **Decode.Data:** Padrão *Unit of Work* (`IUnitOfWork`) e gerenciamento de conexões transacionais via `DbSession` (usando Dapper/ADO.NET).
   - **Decode.Notifications:** Padrão *Domain Notification* para validações e capturas de erros de negócio através do `IDomainNotificationContext`, evitando lançar exceções para fluxos de negócio esperados.
   - **Decode.AspNetCore:** Padronização de respostas de API com a classe unificada `ApiResponse<T>` e tratamento de erros globais com `GlobalExceptionMiddleware`.

## 🏗️ Comandos Úteis do Workspace
Sempre execute ou proponha estes comandos para testar e validar suas alterações:
- **Compilar a Solução:** `dotnet build src/Decode.sln`
- **Executar Testes:** `dotnet test src/Decode.sln` (se disponíveis)
- **Empacotar Pacotes NuGet:** `dotnet pack src/Decode.sln -c Release -o ./nupkg`

## 📂 Documentação das Bibliotecas (READMEs)
Para entender o propósito específico e ver exemplos de uso de cada biblioteca, consulte seus respectivos arquivos de documentação:
- **[Decode.AspNetCore](file:///D:/Projetos/Decode/Libraries/src/Decode.AspNetCore/README.md):** Infraestrutura para APIs (base controller, exception middleware, ApiResponse).
- **[Decode.Cryptography](file:///D:/Projetos/Decode/Libraries/src/Decode.Cryptography/README.md):** Hashing PBKDF2/SHA256, HMAC e criptografia rápida.
- **[Decode.Data](file:///D:/Projetos/Decode/Libraries/src/Decode.Data/README.md):** Implementação concreta de Unit of Work e DbSession (ADO.NET/Dapper).
- **[Decode.Data.Abstractions](file:///D:/Projetos/Decode/Libraries/src/Decode.Data.Abstractions/README.md):** Interfaces e contratos para transações e sessões de banco.
- **[Decode.Extensions](file:///D:/Projetos/Decode/Libraries/src/Decode.Extensions/README.md):** Extensões utilitárias de objetos e Enums.
- **[Decode.Extensions.Options](file:///D:/Projetos/Decode/Libraries/src/Decode.Extensions.Options/README.md):** Validação estática de injeção de opções (IOptions).
- **[Decode.Notifications](file:///D:/Projetos/Decode/Libraries/src/Decode.Notifications/README.md):** Padrão Domain Notification para gestão de erros de negócio.
- **[Decode.Notifications.FluentValidation](file:///D:/Projetos/Decode/Libraries/src/Decode.Notifications.FluentValidation/README.md):** Extensões para integrar validações do FluentValidation.
- **[Decode.Security.Jwt](file:///D:/Projetos/Decode/Libraries/src/Decode.Security.Jwt/README.md):** Geração e validação de tokens JWT.
- **[Decode.Security.Jwt.Abstractions](file:///D:/Projetos/Decode/Libraries/src/Decode.Security.Jwt.Abstractions/README.md):** Contratos e configurações de token JWT.
- **[Decode.Security.ApiKey](file:///D:/Projetos/Decode/Libraries/src/Decode.Security.ApiKey/README.md):** Middleware e autenticação por API Key para ASP.NET Core.
- **[Decode.Security.ApiKey.Abstractions](file:///D:/Projetos/Decode/Libraries/src/Decode.Security.ApiKey.Abstractions/README.md):** Abstrações e contratos de validação para API Keys.
- **[Decode.Storage.Abstractions](file:///D:/Projetos/Decode/Libraries/src/Decode.Storage.Abstractions/README.md):** Abstrações de armazenamento e validador de assinaturas (Magic Numbers).
- **[Decode.Storage.FileSystem](file:///D:/Projetos/Decode/Libraries/src/Decode.Storage.FileSystem/README.md):** Provedor de armazenamento em FileSystem local com escrita atômica.
- **[Decode.Storage.AzureBlob](file:///D:/Projetos/Decode/Libraries/src/Decode.Storage.AzureBlob/README.md):** Provedor de armazenamento em nuvem no Azure Blob Storage com streaming.
- **[Decode.Telemetry](file:///D:/Projetos/Decode/Libraries/src/Decode.Telemetry/README.md):** Integração unificada de OpenTelemetry para rastreabilidade e métricas de desempenho.

## 🤖 Regras de Comportamento para Agentes de IA
- **Preservação de Documentação:** Preserve todos os comentários XML existentes e docstrings que não forem diretamente afetados pelas suas alterações.
- **Compilação e Validação:** Após qualquer modificação em arquivos C# (`.cs`) ou arquivos de projeto (`.csproj`), sempre valide se o projeto compila sem erros executando `dotnet build src/Decode.sln`.
- **Mensagens de Commit:** Se for realizar commits, use o padrão Conventional Commits (ex: `feat(data): add new connection options`, `fix(notifications): resolve validation offset`).
