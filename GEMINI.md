# Gemini Context: Decode Libraries

Este arquivo fornece contexto e diretrizes para o desenvolvimento no ecossistema de bibliotecas **Decode**.

## 📖 Visão Geral do Projeto

As **Decode Libraries** são um ecossistema de bibliotecas .NET modernas, leves e de alta performance, projetadas para acelerar o desenvolvimento de aplicações através de padrões de design reutilizáveis e utilitários robustos.

### Bibliotecas Principais:
- **Decode.Data**: Implementação agnóstica de *Unit of Work* e *DbSession* para gerenciamento de conexões e transações (ADO.NET/Dapper).
- **Decode.Cryptography**: Utilitários de criptografia moderna (PBKDF2, HMAC, SHA256).
- **Decode.Notifications**: Sistema de notificações de domínio para validação e captura de erros de lógica de negócio.
- **Decode.Security.Jwt**: Serviços para geração e validação de tokens JWT.
- **Decode.AspNetCore**: Infraestrutura para APIs (Middleware de exceção global e Controller base).
- **Decode.Extensions**: Métodos de extensão para objetos, opções e configuração.

## 🛠️ Tecnologias e Frameworks

- **Linguagem:** C# 12+ (aproveitando recursos de C# 13/14 conforme disponível no .NET 9/10).
- **Frameworks Alvo:** 
  - .NET Standard 2.1 (Abstrações e utilitários base).
  - .NET 8 / .NET 9 / .NET 10 (Implementações de alta performance).
- **Gerenciamento de Dependências:** NuGet.
- **Padrões:** Dependency Injection, Options Pattern, Domain Notification Pattern, Repository/UoW.

## 🏗️ Comandos Úteis (CLI)

### Compilação
```powershell
dotnet build src/Decode.sln
```

### Empacotamento NuGet
```powershell
dotnet pack src/Decode.sln -c Release -o ./nupkg
```

## 📏 Convenções de Desenvolvimento

### Estilo de Código e Idioma
- **Idioma:** Todo o código, comentários XML e mensagens internas **DEVEM** ser em **Inglês**.
- **File-scoped Namespaces:** Use `namespace MyNamespace;`.
- **Nullable Reference Types:** Sempre habilitado.
- **Modern C#:** Prefira o uso de *Primary Constructors*, *Collection Expressions* e *Pattern Matching*.

### Padronização de APIs (Decode.AspNetCore)
- **Resposta Padrão:** Todas as APIs devem retornar a classe `ApiResponse`, que unifica o formato: `{ success, data, errors, errorId }`.
- **Tratamento de Erros:**
  - **Erros de Negócio:** Devem ser tratados via `IDomainNotificationContext`. O `ApiControllerBase` intercepta essas notificações automaticamente.
  - **Erros Inesperados:** São capturados pelo `GlobalExceptionMiddleware`, que loga o erro e retorna um `errorId` para rastreabilidade.
- **Configuração:** Use `services.AddDecodeAspNetCore()` no `Program.cs` e `app.UseGlobalExceptionMiddleware()` no pipeline.

### Documentação
- Todo método ou classe pública **DEVE** conter comentários XML (`/// <summary>`) detalhando seu propósito e parâmetros em Inglês.

## 📂 Estrutura de Diretórios

- `/src`: Projetos da solução.
  - `Decode.*`: Implementações.
  - `Decode.*.Abstractions`: Interfaces e contratos.
- `/nupkg`: Artefatos de build de pacotes.
