# Decode Libraries — Product Backlog & Roadmaps

Este arquivo serve como repositório de ideias, melhorias e novas bibliotecas sugeridas para expandir o ecossistema de bibliotecas **Decode**.

---

## 🚀 Concluído

### 1. `Decode.Security.ApiKey`
Implementação de middleware e suporte nativo a autenticação via API Keys no ASP.NET Core.
* **Status:** ✅ Concluído (Versão `1.0.3`)
* **Projetos criados:** `Decode.Security.ApiKey` e `Decode.Security.ApiKey.Abstractions`
* **Caminho:** [src/Decode.Security.ApiKey](file:///D:/Projetos/Decode/Libraries/src/Decode.Security.ApiKey/)

---

## 💡 Próximas Ideias (Backlog)

### 2. `Decode.Data.Outbox` (Transactional Outbox Pattern)
Garantia de consistência eventual em sistemas distribuídos/mensageria integrando-se diretamente ao Unit of Work do banco de dados.
* **Objetivo:** Permitir salvar eventos de integração na tabela de outbox usando a mesma conexão/transação do banco de dados onde a entidade de negócio foi alterada. Um `BackgroundService` lê e publica no broker de mensageria (RabbitMQ, Kafka, Azure Service Bus, etc.).
* **Benefícios:** Evita inconsistências (ex: salvar o registro no banco de dados mas falhar ao enviar para a fila).
* **Integração:** Depende diretamente de `Decode.Data.Abstractions` (`IUnitOfWork` e `IDbSession`).

### 3. `Decode.Caching` (Hybrid Caching Utility)
Wrapper simplificado e de alto desempenho para gerenciamento de cache local e distribuído.
* **Objetivo:** Integrar com o novo `HybridCache` do .NET 9+, fornecendo padrões prontos de *Cache-Aside*, invalidação inteligente e serialização otimizada.
* **Benefícios:** Facilidade de uso e proteção nativa contra *Cache Stampede* (múltiplas requisições batendo no banco de dados ao mesmo tempo para recalcular a mesma chave expirada).
* **Integração:** Pode atuar como decorador sobre chamadas de consulta do `IDbSession` em repositórios.

### 4. `Decode.Notifications.ValidationPipeline` (Auto-Validation Middleware)
Validação de requests automática e integrada ao contexto de notificações de domínio antes da execução do controller.
* **Objetivo:** Filtro de Action global ou Middleware no ASP.NET Core que intercepta requests com validações FluentValidation falhas e as joga diretamente no `IDomainNotificationContext` sem que o desenvolvedor precise chamar `.ValidateAsync()` na mão nos serviços.
* **Benefícios:** Menos código boilerplate nos métodos das APIs.
* **Integração:** Conecta os pacotes `Decode.Notifications.FluentValidation` e `Decode.AspNetCore`.

### 5. `Decode.Telemetry` (Observability Pack)
Padronização de logs estruturados, métricas e rastreamento distribuído (Traces).
* **Objetivo:** Fornecer suporte nativo ao **OpenTelemetry** configurando de forma transparente atividades (Activities) para transações do `Decode.Data`, métricas de tempo de resposta em geração de JWT e contadores de Domain Notifications emitidos na API.
* **Benefícios:** Visibilidade total e profissional das bibliotecas em produção via ferramentas como Jaeger, Prometheus ou Datadog.
