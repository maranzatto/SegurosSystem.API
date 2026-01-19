using ContratacaoService.Application.UseCases;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Common;
using Moq;

namespace ContratacaoService.Tests.Application.UseCases;

public class ContractPolicyUseCaseTests
{
    private readonly Mock<IProposalHttpClient> _proposalClientMock;
    private readonly Mock<IPolicyRepository> _policyRepositoryMock;
    private readonly Mock<IClock> _clockMock;
    private readonly ContractPolicyUseCase _useCase;
    private readonly DateTime _fixedDateTime;

    public ContractPolicyUseCaseTests()
    {
        _proposalClientMock = new Mock<IProposalHttpClient>();
        _policyRepositoryMock = new Mock<IPolicyRepository>();
        _clockMock = new Mock<IClock>();
        
        _fixedDateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        _clockMock.Setup(x => x.UtcNow).Returns(_fixedDateTime);
        
        _useCase = new ContractPolicyUseCase(
            _proposalClientMock.Object,
            _policyRepositoryMock.Object,
            _clockMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WithApprovedProposal_ShouldCreatePolicySuccessfully()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var proposalDto = new ProposalDto
        {
            Id = proposalId,
            Status = ProposalStatusContract.Approved,
            StatusName = "Approved",
            Description = "Test Proposal Description"
        };

        _proposalClientMock
            .Setup(x => x.GetByIdAsync(proposalId))
            .ReturnsAsync(proposalDto);

        _policyRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Policy>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.ExecuteAsync(proposalId);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _proposalClientMock.Verify(x => x.GetByIdAsync(proposalId), Times.Once);
        _policyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Policy>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonApprovedProposal_ShouldThrowDomainException()
    {
        // Arrange
        var proposalId = Guid.NewGuid();
        var proposalDto = new ProposalDto
        {
            Id = proposalId,
            Status = ProposalStatusContract.UnderReview,
            StatusName = "Under Review",
            Description = "Test Proposal Description"
        };

        _proposalClientMock
            .Setup(x => x.GetByIdAsync(proposalId))
            .ReturnsAsync(proposalDto);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ContratacaoService.Domain.Exceptions.DomainException>(
            () => _useCase.ExecuteAsync(proposalId));
        
        Assert.Equal("Proposta não aprovada não pode ser contratada.", exception.Message);
        _proposalClientMock.Verify(x => x.GetByIdAsync(proposalId), Times.Once);
        _policyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Policy>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithNonExistentProposal_ShouldThrowException()
    {
        // Arrange
        var proposalId = Guid.NewGuid();

        _proposalClientMock
            .Setup(x => x.GetByIdAsync(proposalId))
            .ReturnsAsync((ProposalDto?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _useCase.ExecuteAsync(proposalId));
        _proposalClientMock.Verify(x => x.GetByIdAsync(proposalId), Times.Once);
        _policyRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Policy>()), Times.Never);
    }

    [Fact]
    public void Constructor_WithNullProposalClient_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ContractPolicyUseCase(
                null!,
                _policyRepositoryMock.Object,
                _clockMock.Object));
        
        Assert.Equal("proposalClient", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullPolicyRepository_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ContractPolicyUseCase(
                _proposalClientMock.Object,
                null!,
                _clockMock.Object));
        
        Assert.Equal("policyRepository", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullClock_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new ContractPolicyUseCase(
                _proposalClientMock.Object,
                _policyRepositoryMock.Object,
                null!));
        
        Assert.Equal("clock", exception.ParamName);
    }
}
