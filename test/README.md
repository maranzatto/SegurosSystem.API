# Testes para SegurosSystem

Este diretório contém os testes unitários para os serviços ContratacaoService e PropostaService.

## Estrutura dos Testes

### ContratacaoService.Tests
- **Domain/Entities/**: Testes para as entidades de domínio (Policy)
- **Domain/ValueObjects/**: Testes para os objetos de valor (PolicyNumber)
- **Application/UseCases/**: Testes para os casos de uso (ContractPolicyUseCase)
- **Api/Controllers/**: Testes para os controladores API (PolicyCommandController)

### PropostaService.Tests
- **Domain/Entities/**: Testes para as entidades de domínio (Proposal)
- **Domain/ValueObjects/**: Testes para os objetos de valor (ProposalDescription, RejectionReason)
- **Application/UseCases/**: Testes para os casos de uso (CreateProposalUseCase)
- **Api/Controllers/**: Testes para os controladores API (ProposalCommandController)

## Tecnologias Utilizadas

- **xUnit**: Framework de testes
- **Moq**: Biblioteca para criação de mocks
- **Microsoft.AspNetCore.Mvc.Testing**: Para testes de controllers
- **FluentAssertions**: Para assertions mais legíveis (se adicionado)

## Executando os Testes

### Para executar todos os testes:
```bash
cd teste
dotnet test
```

### Para executar testes de um projeto específico:
```bash
cd teste/ContratacaoService.Tests
dotnet test

cd teste/PropostaService.Tests
dotnet test
```

### Para executar testes com detalhes:
```bash
dotnet test --verbosity normal
```

## Cobertura de Testes

Os testes cobrem:

### Camada de Domínio
- ✅ Validações de entidades
- ✅ Regras de negócio
- ✅ Objetos de valor
- ✅ Exceções de domínio

### Camada de Aplicação
- ✅ Casos de uso
- ✅ Fluxos de negócio
- ✅ Integração entre serviços

### Camada de API
- ✅ Controllers
- ✅ Retornos HTTP
- ✅ Propagação de exceções

## Boas Práticas Aplicadas

1. **AAA Pattern**: Arrange, Act, Assert em todos os testes
2. **Nomenclatura descritiva**: Testes com nomes claros e explicativos
3. **Testes independentes**: Cada teste é isolado e não depende de outros
4. **Mocks**: Uso de Moq para isolar dependências
5. **Cobertura de exceções**: Testes para casos de sucesso e falha
6. **Testes de borda**: Valores limites e casos extremos

## Adicionando Novos Testes

Ao adicionar novos testes, siga este padrão:

```csharp
[Fact]
public void MethodName_Condition_ExpectedResult()
{
    // Arrange
    // Configurar o cenário de teste
    
    // Act
    // Executar a ação a ser testada
    
    // Assert
    // Verificar o resultado esperado
}
```

## Dependências

- .NET 8.0
- xUnit 2.4.2
- Moq 4.20.72
- Microsoft.AspNetCore.Mvc.Testing 10.0.2
