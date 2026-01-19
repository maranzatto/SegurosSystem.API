using PropostaService.Application.UseCases;
using PropostaService.Application.Interfaces;
using PropostaService.Application.Interfaces.Repositories;
using PropostaService.Application.DTOs;
using PropostaService.Domain.ValueObjects;
using PropostaService.Domain.Common;
using Moq;

namespace PropostaService.Tests.Application.UseCases;

public class CreateProposalUseCaseTests
{
    private readonly Mock<IProposalRepository> _repositoryMock;
    private readonly Mock<IClock> _clockMock;
    private readonly CreateProposalUseCase _useCase;
    private readonly DateTime _fixedDateTime;

    public CreateProposalUseCaseTests()
    {
        _repositoryMock = new Mock<IProposalRepository>();
        _clockMock = new Mock<IClock>();
        
        _fixedDateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        _clockMock.Setup(x => x.UtcNow).Returns(_fixedDateTime);
        
        _useCase = new CreateProposalUseCase(_repositoryMock.Object, _clockMock.Object);
    }

    [Fact]
    public async Task Execute_WithValidRequest_ShouldCreateProposalSuccessfully()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = new ProposalDescription("Test proposal description")
        };

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Proposal>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Proposal>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithValidRequest_ShouldCreateProposalWithCorrectProperties()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = new ProposalDescription("Test proposal description")
        };

        Proposal capturedProposal = null!;
        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Proposal>()))
            .Callback<Proposal>(p => capturedProposal = p)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(request);

        // Assert
        Assert.NotNull(capturedProposal);
        Assert.Equal(result, capturedProposal.Id);
        Assert.Equal(request.Description, capturedProposal.Description);
        Assert.Equal(_fixedDateTime, capturedProposal.CreatedAt);
        Assert.Equal(_fixedDateTime, capturedProposal.UpdatedAt);
        Assert.False(capturedProposal.IsDeleted);
        Assert.Null(capturedProposal.RejectionReason);
    }

    [Fact]
    public async Task Execute_WithNullDescription_ShouldStillCreateProposal()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = null!
        };

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Proposal>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _useCase.Execute(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Proposal>()), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullRepository_ShouldNotThrow()
    {
        // Act & Assert - Should not throw when repository is null
        // (Note: This might indicate a design issue, but testing current behavior)
        var useCase = new CreateProposalUseCase(null!, _clockMock.Object);
        Assert.NotNull(useCase);
    }

    [Fact]
    public void Constructor_WithNullClock_ShouldNotThrow()
    {
        // Act & Assert - Should not throw when clock is null
        // (Note: This might indicate a design issue, but testing current behavior)
        var useCase = new CreateProposalUseCase(_repositoryMock.Object, null!);
        Assert.NotNull(useCase);
    }

    [Fact]
    public async Task Execute_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = new ProposalDescription("Test proposal description")
        };

        var expectedException = new InvalidOperationException("Database error");
        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Proposal>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.Execute(request));
        Assert.Equal(expectedException, exception);
    }
}
