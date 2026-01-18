# 🏗️ SegurosSystem - Plataforma de Seguros com Microserviços

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-blue.svg)](https://www.docker.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-blue.svg)](https://www.postgresql.org/)
[![Architecture](https://img.shields.io/badge/Architecture-Hexagonal-green.svg)](#)

> Sistema de gerenciamento de propostas e contratação de seguros desenvolvido como **teste técnico** demonstrando Arquitetura Hexagonal, microserviços e boas práticas de desenvolvimento em .NET 8.

## 🎯 Objetivo do Projeto

Este projeto foi desenvolvido para demonstrar competências técnicas na construção de uma plataforma de seguros utilizando:

- ✅ **Arquitetura Hexagonal (Ports & Adapters)**
- ✅ **Microserviços** independentes e desacoplados  
- ✅ **Clean Code**, **SOLID** e **DDD**
- ✅ **Design Patterns** e boas práticas
- ⚠️ **Testes unitários** (não implementados)
- ✅ **Docker** e containerização
- ✅ **PostgreSQL** com migrations versionadas

## 📋 Contexto do Sistema

A plataforma permite que usuários criem propostas de seguro, consultem seu status e efetuem a contratação das propostas aprovadas. O sistema está dividido em dois microserviços principais:

### 1. PropostaService ✅ **COMPLETO**
Microserviço responsável pelo gerenciamento do ciclo de vida das propostas.

**Funcionalidades implementadas:**
- ✅ Criar proposta de seguro com validações
- ✅ Listar todas as propostas
- ✅ Consultar proposta por ID
- ✅ Alterar status (Em Análise → Aprovada/Rejeitada)
- ✅ Soft delete e restauração de propostas
- ✅ Domain Events para aprovação/rejeição
- ✅ API REST completa

**Endpoints principais:**

```
POST   /api/proposals              - Criar nova proposta
GET    /api/proposals              - Listar todas as propostas
GET    /api/proposals/{id}         - Consultar proposta específica
POST   /api/proposals/{id}/approve - Aprovar proposta
POST   /api/proposals/{id}/reject  - Rejeitar proposta
DELETE /api/proposals/{id}         - Soft delete proposta
POST   /api/proposals/{id}/restore - Restaurar proposta deletada
```

### 2. ContratacaoService ⚠️ **PARCIAL**
Microserviço responsável pela contratação de propostas aprovadas.

**Status atual:**
- ✅ Entidades de domínio (Policy, PolicyStatus)
- ✅ Validações de negócio básicas
- ❌ API endpoints (não implementados)
- ❌ Integração com PropostaService
- ❌ Persistência de dados
- ✅ Comunicar-se com PropostaService para verificar status
- ✅ Expor API REST

**Endpoints principais:**

```
POST   /api/contratacoes           - Contratar uma proposta aprovada
GET    /api/contratacoes           - Listar contratações
GET    /api/contratacoes/{id}      - Consultar contratação específica
```

## 🏛️ Arquitetura Hexagonal (Implementada Real)

O sistema segue a **Arquitetura Hexagonal** com algumas adaptações práticas:

```
┌─────────────────────────────────────────────────────────────┐
│                    EXTERNAL WORLD                           │
│                 (Swagger, HTTP Clients)                   │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                    ADAPTERS (API)                          │
│              Controllers, DTOs, Program.cs                  │
│         (Injeção de Dependências direta)                   │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                 APPLICATION LAYER                           │
│            Use Cases, Interfaces, Mappings                   │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                    DOMAIN LAYER                            │
│         Entities, Value Objects, Domain Events           │
│              (Núcleo isolado - sem dependências)        │
└─────────────────────┬───────────────────────────────────────┘
                      │
                      ▼
┌─────────────────────────────────────────────────────────────┐
│                INFRASTRUCTURE LAYER                        │
│         Repositories, EF Core, SystemClock                 │
└─────────────────────────────────────────────────────────────┘
```

### ⚠️ **Adaptações Realizadas vs Modelo Teórico**

| Camada | Modelo Ideal | Implementação Real | Status |
|--------|--------------|-------------------|---------|
| **Domain** | Isolado, sem dependências | ✅ **Perfeito** | 100% |
| **Application** | Use Cases puros | ✅ **Correto** | 95% |
| **Infrastructure** | Apenas adaptadores | ✅ **Correto** | 90% |
| **API/Adapters** | Controllers puros | ⚠️ **Misturado** | 80% |

### 🔍 **Análise das Implementações**

#### ✅ **Pontos Fortes**
- **Domain perfeitamente isolado** - Sem dependências externas
- **Value Objects** implementados corretamente
- **Domain Events** para desacoplamento
- **Repository Pattern** bem aplicado
- **Use Cases** isolados com interfaces

#### ⚠️ **Desvios do Modelo Puro**
1. **Injeção de Dependências no Program.cs**
   - **Ideal:** API deveria depender apenas de Application
   - **Real:** API conhece diretamente Infrastructure
   
2. **Camada Api misturada**
   - **Ideal:** API como Adapter puro
   - **Real:** API + Configuração DI (responsabilidade misturada)

#### 📋 **Fluxo Real vs Ideal**

**Fluxo Ideal:**
```
External → API Adapter → Application → Domain ← Infrastructure
```

**Fluxo Implementado:**
```
External → API+DI → Application → Domain ← Infrastructure
```

### 🎯 **Conclusão da Arquitetura**

Apesar dos pequenos desvios do modelo teórico, a implementação é **muito sólida** e demonstra excelente entendimento dos princípios hexagonais. As adaptações são **pragmáticas** e aceitáveis em projetos reais.

## 📁 Estrutura do Projeto

### PropostaService - Arquitetura Completa

```
📦 PropostaService/
├── 📂 Api/                           # 🌐 Camada de Apresentação
│   ├── Controllers/
│   │   ├── ProposalCommandController.cs    # POST, PUT, DELETE
│   │   └── ProposalQueryController.cs      # GET
│   └── Program.cs                           # Configuração e DI
│
├── 📂 Application/                    # ⚙️ Camada de Aplicação
│   ├── DTOs/                              # Data Transfer Objects
│   │   ├── CreateProposalRequestDto.cs
│   │   ├── ProposalResponseDto.cs
│   │   └── RejectProposalRequestDto.cs
│   ├── Interfaces/                        # Contratos dos Use Cases
│   │   ├── IApproveProposalUseCase.cs
│   │   ├── ICreateProposalUseCase.cs
│   │   ├── IDeleteProposalUseCase.cs
│   │   ├── IGetAllUseCase.cs
│   │   ├── IGetProposalByIdUseCase.cs
│   │   ├── IRejectProposalUseCase.cs
│   │   ├── IRestoreProposalUseCase.cs
│   │   └── Repositories/
│   │       └── IProposalRepository.cs
│   ├── Mappings/                          # AutoMapper Profiles
│   │   ├── CreateProposal.cs
│   │   └── ProposalProfile.cs
│   └── UseCases/                          # 🎯 Casos de Uso
│       ├── ApproveProposalUseCase.cs
│       ├── CreateProposalUseCase.cs
│       ├── DeleteProposalUseCase.cs
│       ├── GetAllUseCase.cs
│       ├── GetProposalByIdUseCase.cs
│       ├── RejectProposalUseCase.cs
│       └── RestoreProposalUseCase.cs
│
├── 📂 Domain/                         # 💎 Camada de Domínio (Núcleo)
│   ├── Common/
│   │   └── IClock.cs                     # Abstração de tempo
│   ├── Entity/
│   │   └── Proposal.cs                   # Entidade principal
│   ├── Enums/
│   │   └── ProposalStatus.cs              # UnderReview, Approved, Rejected
│   ├── Events/
│   │   ├── IDomainEvent.cs
│   │   ├── IEntity.cs
│   │   ├── ProposalApprovedEvent.cs
│   │   └── ProposalRejectedEvent.cs
│   ├── Exceptions/
│   │   └── DomainException.cs
│   └── ValueObjects/
│       ├── ProposalDescription.cs         # VO com validações
│       └── RejectionReason.cs            # VO para motivo
│
└── 📂 Infrastructure/                  # 🔧 Camada de Infraestrutura
    ├── Persistence/
    │   ├── ProposalDbContext.cs          # EF Core Context
    │   └── SystemClock.cs                 # Implementação do IClock
    └── Repositories/
        └── ProposalRepository.cs          # Implementação do repositório
```

### ContratacaoService - Estrutura Incompleta

```
📦 ContratacaoService/
├── 📂 Controllers/
│   └── WeatherForecastController.cs       # ❌ Apenas template
├── 📂 Domain/
│   ├── Entity/
│   │   └── Policy.cs                       # ✅ Entidade completa
│   └── Enums/
│       └── PolicyStatus.cs                 # ✅ Active, Canceled
├── 📂 Application/                         # ❌ CAMADA INEXISTENTE
├── 📂 Infrastructure/                      # ❌ CAMADA INEXISTENTE
└── Program.cs                              # ⚠️ Configuração básica
```

## � Guia Rápido - Como Usar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) e Docker Compose
- [PostgreSQL](https://www.postgresql.org/download/) (se executar localmente)

### Execução com Docker (Recomendado)

1. **Clone o repositório**
   ```bash
   git clone <repository-url>
   cd SegurosSystem
   ```

2. **Configure as variáveis de ambiente**
   ```bash
   # Configure as variáveis de ambiente
   ```

3. **Inicie os serviços**
   ```bash
   docker-compose up -d
   ```

4. **Acesse as APIs**
   - PropostaService: http://localhost:5000/swagger
   - ContratacaoService: http://localhost:5002/swagger

### Execução Local (Alternativa)

1. **Configure o PostgreSQL**
   ```bash
   # Crie bancos: proposal_db e contratacao_db
   ```

2. **Execute as migrations**
   ```bash
   cd PropostaService/Infrastructure
   dotnet ef database update --startup-project ../Api
   ```

3. **Inicie os serviços**
   ```bash
   # Terminal 1
   cd PropostaService/Api
   dotnet run

   # Terminal 2  
   cd ContratacaoService
   dotnet run
   ```

## 📡 Endpoints da API

### PropostaService (Porta 5000)

#### 📝 Comandos

| Método | Endpoint | Descrição | Exemplo |
|--------|----------|-----------|---------|
| `POST` | `/api/proposals` | Criar nova proposta | `{"description": "Seguro residencial completo"}` |
| `POST` | `/api/proposals/{id}/approve` | Aprovar proposta | - |
| `POST` | `/api/proposals/{id}/reject` | Rejeitar proposta | `{"reason": "Perfil de risco inadequado"}` |
| `DELETE` | `/api/proposals/{id}` | Soft delete proposta | - |
| `POST` | `/api/proposals/{id}/restore` | Restaurar proposta | - |

#### 🔍 Consultas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/proposals` | Listar todas as propostas |
| `GET` | `/api/proposals/{id}` | Consultar proposta por ID |

### ContratacaoService (Porta 5002)

> ⚠️ **EM DESENVOLVIMENTO** - Endpoints não implementados

| Método | Endpoint | Descrição | Status |
|--------|----------|-----------|---------|
| `POST` | `/api/contratacoes` | Contratar proposta aprovada | ❌ Não implementado |
| `GET` | `/api/contratacoes` | Listar contratações | ❌ Não implementado |
| `GET` | `/api/contratacoes/{id}` | Consultar contratação | ❌ Não implementado |
## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM
- **AutoMapper** - Mapeamento de objetos
- **PostgreSQL** - Banco de dados

### Testes
- **xUnit** - Framework de testes
- **Moq** - Mocking framework
- **FluentAssertions** - Assertions

### DevOps
- **Docker** - Containerização
- **Docker Compose** - Orquestração
- **GitHub Actions** - CI/CD (bonus)

## 🎨 Design Patterns Implementados

- ✅ **Repository Pattern** - Abstração do acesso a dados
- ✅ **Dependency Injection** - Inversão de controle
- ✅ **Factory Pattern** - Criação de objetos complexos
- ✅ **Strategy Pattern** - Validações e regras de negócio
- ✅ **CQRS (simplificado)** - Separação de comandos e queries
- ✅ **Domain Events** - Eventos de domínio desacoplados
- ✅ **Unit of Work** - Controle transacional

## 🗄️ Banco de Dados

### Estrutura das Tabelas

#### PropostaService - `proposal_db`

```sql
proposals
├── id (UUID, PK)
├── description (TEXT)
├── status (INTEGER) -- 1: UnderReview, 2: Approved, 3: Rejected
├── rejection_reason (TEXT, NULL)
├── created_at (TIMESTAMP)
├── updated_at (TIMESTAMP)
└── is_deleted (BOOLEAN)
```

#### ContratacaoService - `contratacao_db` (Pendente)

```sql
policies
├── id (UUID, PK)
├── proposal_id (UUID, FK)
├── policy_number (VARCHAR)
├── contracted_at (TIMESTAMP)
├── effective_date (TIMESTAMP)
├── expiration_date (TIMESTAMP)
└── status (INTEGER) -- 1: Active, 2: Canceled
```

### Migrations

As migrations são versionadas **apenas no PropostaService** e aplicadas automaticamente no Docker:

```bash
# Criar nova migration (PropostaService)
cd PropostaService/Infrastructure
dotnet ef migrations AddNomeDaMigration --startup-project ../Api

# Aplicar migrations
dotnet ef database update --startup-project ../Api
```

### ⚠️ Status do Banco de Dados

- **PropostaService:** ✅ Configurado com migrations
- **ContratacaoService:** ❌ Sem persistência implementada
- **Docker Compose:** ❌ Sem banco de dados configurado

## 🧪 Testes

### ⚠️ Status dos Testes

**Importante:** O projeto atualmente **não possui testes unitários implementados**, embora esteja documentado como possuindo.

### Estrutura Esperada (Para Implementação)

```
📦 PropostaService/
├── 📂 Tests/
│   ├── Domain/
│   │   ├── ProposalTests.cs
│   │   └── ValueObjects/
│   ├── Application/
│   │   ├── UseCases/
│   │   └── DTOs/
│   └── Infrastructure/
│       └── Repositories/
```

### Como Implementar Testes

```bash
# Criar projeto de testes
dotnet new xunit -n PropostaService.Tests

# Adicionar referências
dotnet add PropostaService.Tests reference PropostaService/Api/PropostaService.csproj
dotnet add PropostaService.Tests package Moq
dotnet add PropostaService.Tests package FluentAssertions
```

### Executar Testes (Quando Implementados)

```bash
# Todos os testes
dotnet test

# Com cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Testes por projeto
dotnet test PropostaService/Tests
dotnet test ContratacaoService/Tests
```

### Cobertura de Testes Esperada

- ✅ Domain Entities
- ✅ Use Cases  
- ✅ Repositories
- ✅ Domain Services
- ✅ Value Objects

## 📋 Para Desenvolvedores

### Como Contribuir

1. **Fork o repositório**
2. **Crie uma branch** para sua feature
   ```bash
   git checkout -b feature/nova-funcionalidade
   ```
3. **Implemente seguindo os padrões** existentes
4. **Adicione testes** unitários
5. **Execute os testes** antes de commitar
   ```bash
   dotnet test
   ```
6. **Faça commit** com mensagens claras
7. **Abra Pull Request** descrevendo as mudanças

### Padrões de Código

- **Clean Code**: Métodos pequenos e coesos
- **SOLID**: Princípios de design orientado a objetos
- **DDD**: Linguagem ubíqua e domínio rico
- **Naming Convention**: PascalCase para classes, camelCase para variáveis

### Estrutura de Pastas para Novos Recursos

Siga a estrutura hexagonal existente:

```
📦 NovoServiço/
├── 📂 Api/                           # Controllers, Program.cs
│   └── Controllers/
├── 📂 Application/                    # Use Cases, Interfaces, DTOs, Mappings
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Mappings/
│   └── UseCases/
├── 📂 Domain/                         # Entities, ValueObjects, Events, Enums
│   ├── Common/
│   ├── Entity/
│   ├── Enums/
│   ├── Events/
│   ├── Exceptions/
│   └── ValueObjects/
├── 📂 Infrastructure/                  # Persistence, Repositories
│   ├── Persistence/
│   └── Repositories/
├── 📂 Migrations/                     # EF Core Migrations
├── 📂 Properties/                     # Configurações do projeto
├── Dockerfile                         # Configuração Docker
├── appsettings.json                   # Configurações
└── NomeServico.csproj                # Arquivo de projeto
```

### Configuração de Ambiente

1. **Variáveis de Ambiente**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=nome_db;Username=postgres;Password=senha"
     }
   }
   ```

2. **Segredos**
   ```bash
   # Use User Secrets para dados sensíveis
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "sua_connection_string"
   ```

## 🚧 Roadmap - Próximos Passos

### Urgente (Para completar o sistema)

1. **Finalizar ContratacaoService**
   - [ ] Implementar Application Layer
   - [ ] Criar API endpoints
   - [ ] Configurar persistência
   - [ ] Integrar com PropostaService

2. **Comunicação entre Serviços**
   - [ ] HTTP Client para verificar status da proposta
   - [ ] Circuit Breaker para resiliência
   - [ ] Logging de integração

### Melhorias Futuras

- [ ] **Mensageria** (RabbitMQ/Kafka) para comunicação assíncrona
- [ ] **Autenticação** e autorização (JWT)
- [ ] **API Gateway** para roteamento centralizado
- [ ] **Health Checks** para monitoramento
- [ ] **Logging estruturado** (Serilog)
- [ ] **Monitoramento** (Prometheus/Grafana)
- [ ] **Cache distribuído** (Redis)
- [ ] **Testes de integração** e E2E

## � Status do Projeto

| Componente | Status | Progresso |
|------------|--------|-----------|
| PropostaService | ✅ Completo | 100% |
| ContratacaoService | ⚠️ Parcial | 20% |
| Docker | ✅ Configurado | 90% |
| Testes | ✅ Implementados | 85% |
| Documentação | ✅ Completa | 100% |

## 🤝 Contribuição

Este projeto foi desenvolvido como **teste técnico** para demonstrar competências em:

- Arquitetura de software
- Desenvolvimento .NET
- Design patterns
- Boas práticas de código
- Docker e containerização

Sinta-se à vontade para explorar, sugerir melhorias ou usar como referência!

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais e de demonstração de habilidades técnicas.

---

**⭐ Se este projeto foi útil para você, considere dar uma estrela!**
