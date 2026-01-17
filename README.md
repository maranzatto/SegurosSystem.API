# SegurosSystem

Sistema de gerenciamento de propostas e contratação de seguros, desenvolvido com **Arquitetura Hexagonal (Ports & Adapters)** e microserviços em .NET 8.

## 📋 Sobre o Projeto

Este projeto foi desenvolvido para demonstrar a aplicação de boas práticas de desenvolvimento de software, incluindo:

- ✅ **Arquitetura Hexagonal (Ports & Adapters)**
- ✅ **Microserviços** independentes e desacoplados
- ✅ **Clean Code** e **SOLID**
- ✅ **Domain-Driven Design (DDD)**
- ✅ **Design Patterns**
- ✅ **Testes Unitários**
- ✅ **Docker** e containerização
- ✅ **PostgreSQL** com migrations versionadas

## 🏗️ Arquitetura

O sistema é composto por dois microserviços que se comunicam via HTTP REST:

### 1. PropostaService

Microserviço responsável pelo gerenciamento de propostas de seguro.

**Funcionalidades:**

- ✅ Criar proposta de seguro
- ✅ Listar propostas
- ✅ Consultar proposta por ID
- ✅ Alterar status da proposta (Em Análise, Aprovada, Rejeitada)
- ✅ Expor API REST

**Endpoints principais:**

```
POST   /api/propostas              - Criar nova proposta
GET    /api/propostas              - Listar todas as propostas
GET    /api/propostas/{id}         - Consultar proposta específica
PATCH  /api/propostas/{id}/status  - Alterar status da proposta
```

### 2. ContratacaoService

Microserviço responsável pela contratação de seguros aprovados.

**Funcionalidades:**

- ✅ Contratar proposta (somente se status = Aprovada)
- ✅ Armazenar informações da contratação (ID da proposta, data de contratação)
- ✅ Comunicar-se com PropostaService para verificar status
- ✅ Expor API REST

**Endpoints principais:**

```
POST   /api/contratacoes           - Contratar uma proposta aprovada
GET    /api/contratacoes           - Listar contratações
GET    /api/contratacoes/{id}      - Consultar contratação específica
```

## 🎯 Arquitetura Hexagonal

Cada microserviço segue a arquitetura hexagonal com camadas bem definidas:

```
📦 Service/
├── 📂 Domain/              # Núcleo do negócio (Entities, Value Objects, Domain Services)
│   ├── Entities/          # Entidades do domínio
│   ├── ValueObjects/      # Objetos de valor
│   ├── Enums/            # Enumerações
│   └── Ports/            # Interfaces (Ports) - contratos do domínio
│       ├── IPropostaRepository.cs
│       └── IPropostaService.cs
│
├── 📂 Application/         # Casos de uso e orquestração
│   ├── UseCases/         # Casos de uso da aplicação
│   ├── DTOs/             # Data Transfer Objects
│   └── Services/         # Services de aplicação
│
├── 📂 Infrastructure/      # Adaptadores externos (Adapters)
│   ├── Persistence/      # Repositórios, EF Core, Migrations
│   │   ├── Context/
│   │   ├── Repositories/
│   │   └── Migrations/
│   └── External/         # Integração com APIs externas, HTTP Clients
│
└── 📂 Api/                # Adapter de entrada (Controllers, Middleware)
    ├── Controllers/
    ├── Filters/
    └── Program.cs
```

**Fluxo da Arquitetura:**

```
[API/Controllers] → [Application/UseCases] → [Domain/Entities + Ports] ← [Infrastructure/Adapters]
```

**Princípios aplicados:**

- 🎯 **Inversão de Dependência**: Domain não depende de nada, Infrastructure depende de Domain
- 🔌 **Ports & Adapters**: Interfaces (Ports) no Domain, implementações (Adapters) na Infrastructure
- 🧩 **Separation of Concerns**: Cada camada com responsabilidade única e bem definida

## 🛠️ Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core** - Web API
- **Entity Framework Core** - ORM e Migrations
- **PostgreSQL** - Banco de dados relacional
- **Docker & Docker Compose** - Containerização
- **xUnit** - Testes unitários
- **Moq** - Mocking para testes
- **FluentAssertions** - Assertions nos testes

## 📊 Diagrama da Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                         CLIENTE                              │
│                     (Postman, Frontend)                      │
└────────────────┬───────────────────────┬────────────────────┘
                 │                       │
                 ▼                       ▼
    ┌────────────────────┐  ┌────────────────────┐
    │  PropostaService   │  │ ContratacaoService │
    │   (Port: 5001)     │  │   (Port: 5002)     │
    │                    │  │                    │
    │  ┌──────────────┐  │  │  ┌──────────────┐ │
    │  │     API      │  │  │  │     API      │ │
    │  └──────┬───────┘  │  │  └──────┬───────┘ │
    │         │          │  │         │         │
    │  ┌──────▼───────┐  │  │  ┌──────▼───────┐ │
    │  │ Application  │  │  │  │ Application  │ │
    │  └──────┬───────┘  │  │  └──────┬───────┘ │
    │         │          │  │         │         │
    │  ┌──────▼───────┐  │  │  ┌──────▼───────┐ │
    │  │   Domain     │  │  │  │   Domain     │ │
    │  └──────┬───────┘  │  │  └──────┬───────┘ │
    │         │          │  │         │         │
    │  ┌──────▼───────┐  │  │  ┌──────▼───────┐ │
    │  │Infrastructure│  │  │  │Infrastructure│ │
    │  └──────┬───────┘  │  │  └──────┬───────┘ │
    └─────────┼──────────┘  └─────────┼─────────┘
              │                       │
              ▼                       ▼
    ┌─────────────────────────────────────────┐
    │         PostgreSQL Database              │
    │  - proposal_db (PropostaService)         │
    │  - contratacao_db (ContratacaoService)   │
    └─────────────────────────────────────────┘

Comunicação HTTP REST: ContratacaoService → PropostaService
```

## 🚀 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) e Docker Compose
- [Git](https://git-scm.com/)

### Passo a passo

1. **Clone o repositório**

```bash
git clone https://github.com/seu-usuario/seguros-system.git
cd seguros-system
```

2. **Configure as variáveis de ambiente (opcional)**

Crie um arquivo `appsettings.Development.json` em cada serviço ou use as configurações padrão do Docker Compose.

3. **Execute com Docker Compose**

```bash
docker-compose up -d
```

Isso irá:

- ✅ Subir o banco PostgreSQL
- ✅ Aplicar as migrations automaticamente
- ✅ Iniciar o PropostaService na porta 5001
- ✅ Iniciar o ContratacaoService na porta 5002

4. **Acesse as APIs**

- PropostaService: http://localhost:5001/swagger
- ContratacaoService: http://localhost:5002/swagger

### Executar sem Docker (alternativa)

1. **Inicie o PostgreSQL localmente**

2. **Configure as connection strings** em `appsettings.Development.json`

3. **Execute as migrations**

```bash
cd PropostaService
dotnet ef database update

cd ../ContratacaoService
dotnet ef database update
```

4. **Execute os serviços**

```bash
# Terminal 1
cd PropostaService/Api
dotnet run

# Terminal 2
cd ContratacaoService/Api
dotnet run
```

## 🗄️ Banco de Dados

### Migrations

As migrations estão versionadas e são aplicadas automaticamente no Docker. Para criar novas migrations:

```bash
# PropostaService
cd PropostaService/Infrastructure
dotnet ef migrations add NomeDaMigration --startup-project ../Api

# ContratacaoService
cd ContratacaoService/Infrastructure
dotnet ef migrations add NomeDaMigration --startup-project ../Api
```

### Estrutura das Tabelas

**PropostaService:**

```sql
Propostas
├── Id (Guid, PK)
├── NomeCliente (varchar)
├── Cpf (varchar)
├── Valor (decimal)
├── Status (int) -- 0: EmAnalise, 1: Aprovada, 2: Rejeitada
├── DataCriacao (timestamp)
└── DataAtualizacao (timestamp)
```

**ContratacaoService:**

```sql
Contratacoes
├── Id (Guid, PK)
├── PropostaId (Guid)
├── DataContratacao (timestamp)
├── NumeroApolice (varchar)
└── Ativa (bool)
```

## 🧪 Testes

Execute os testes unitários:

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

**Cobertura de testes:**

- ✅ Domain Entities
- ✅ Use Cases
- ✅ Repositories
- ✅ Domain Services
- ✅ Integration between services

## 📝 Exemplos de Uso

### 1. Criar uma Proposta

```bash
POST http://localhost:5001/api/propostas
Content-Type: application/json

{
  "nomeCliente": "João Silva",
  "cpf": "12345678900",
  "valor": 50000.00
}
```

**Response:**

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nomeCliente": "João Silva",
  "cpf": "12345678900",
  "valor": 50000.0,
  "status": "EmAnalise",
  "dataCriacao": "2024-01-17T10:30:00"
}
```

### 2. Aprovar Proposta

```bash
PATCH http://localhost:5001/api/propostas/3fa85f64-5717-4562-b3fc-2c963f66afa6/status
Content-Type: application/json

{
  "status": "Aprovada"
}
```

### 3. Contratar Proposta

```bash
POST http://localhost:5002/api/contratacoes
Content-Type: application/json

{
  "propostaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Response:**

```json
{
  "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "propostaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "dataContratacao": "2024-01-17T11:00:00",
  "numeroApolice": "APL-2024-0001",
  "ativa": true
}
```

## 🎨 Design Patterns Aplicados

- **Repository Pattern** - Abstração do acesso a dados
- **Dependency Injection** - Inversão de controle
- **Factory Pattern** - Criação de objetos complexos
- **Strategy Pattern** - Validações e regras de negócio
- **CQRS (simplificado)** - Separação de comandos e queries
- **Unit of Work** - Controle transacional

## ✨ Boas Práticas Implementadas

### Clean Code

- ✅ Nomenclatura clara e descritiva
- ✅ Métodos pequenos e coesos
- ✅ Comentários somente quando necessário
- ✅ Código auto-explicativo

### SOLID

- ✅ **S**ingle Responsibility Principle
- ✅ **O**pen/Closed Principle
- ✅ **L**iskov Substitution Principle
- ✅ **I**nterface Segregation Principle
- ✅ **D**ependency Inversion Principle

### DDD

- ✅ Entities com lógica de negócio
- ✅ Value Objects para conceitos imutáveis
- ✅ Domain Services para operações complexas
- ✅ Repositories como abstração de persistência
- ✅ Linguagem ubíqua no código

## 📁 Estrutura de Diretórios

```
SegurosSystems/
├── 📂 ContratacaoService/
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   └── Tests/
├── 📂 PropostaService/
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   ├── Api/
│   └── Tests/
├── 📄 docker-compose.yml
├── 📄 .dockerignore
├── 📄 .gitignore
└── 📄 README.md
```

## 🐳 Docker

### Dockerfile

Cada serviço possui seu Dockerfile otimizado com multi-stage build.

### Docker Compose

Orquestra todos os serviços necessários:

- PostgreSQL
- PropostaService
- ContratacaoService

```bash
# Iniciar todos os serviços
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar serviços
docker-compose down

# Recriar containers
docker-compose up -d --build
```

## 🔧 Possíveis Melhorias Futuras

- [ ] Implementar mensageria (RabbitMQ/Kafka) para comunicação assíncrona
- [ ] Adicionar autenticação e autorização (JWT)
- [ ] Implementar padrão Saga para transações distribuídas
- [ ] Adicionar API Gateway
- [ ] Implementar Circuit Breaker para resiliência
- [ ] Adicionar logging estruturado (Serilog)
- [ ] Implementar health checks
- [ ] Adicionar monitoramento (Prometheus/Grafana)
- [ ] Implementar cache distribuído (Redis)

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais e de demonstração de habilidades técnicas.

## 👤 Autor

[Seu Nome]

- GitHub: [@seu-usuario](https://github.com/seu-usuario)
- LinkedIn: [Seu Perfil](https://linkedin.com/in/seu-perfil)

---

⭐ Se este projeto foi útil para você, considere dar uma estrela!
