using Microsoft.AspNetCore.Mvc;
using PropostaService.Api.Controllers;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace PropostaService.Tests.Api.Controllers;

public class ProposalCommandControllerTests
{
    private readonly Mock<ICreateProposalUseCase> _createProposalUseCaseMock;
    private readonly Mock<IApproveProposalUseCase> _approveProposalUseCaseMock;
    private readonly Mock<IRejectProposalUseCase> _rejectProposalUseCaseMock;
    private readonly Mock<IDeleteProposalUseCase> _deleteProposalUseCaseMock;
    private readonly Mock<IRestoreProposalUseCase> _restoreProposalUseCaseMock;
    private readonly Mock<ILogger<ProposalCommandController>> _loggerMock;
    private readonly ProposalCommandController _controller;
    private readonly Guid _testProposalId;

    public ProposalCommandControllerTests()
    {
        _createProposalUseCaseMock = new Mock<ICreateProposalUseCase>();
        _approveProposalUseCaseMock = new Mock<IApproveProposalUseCase>();
        _rejectProposalUseCaseMock = new Mock<IRejectProposalUseCase>();
        _deleteProposalUseCaseMock = new Mock<IDeleteProposalUseCase>();
        _restoreProposalUseCaseMock = new Mock<IRestoreProposalUseCase>();
        _loggerMock = new Mock<ILogger<ProposalCommandController>>();
        
        _controller = new ProposalCommandController(
            _createProposalUseCaseMock.Object,
            _approveProposalUseCaseMock.Object,
            _rejectProposalUseCaseMock.Object,
            _deleteProposalUseCaseMock.Object,
            _restoreProposalUseCaseMock.Object,
            _loggerMock.Object);
        
        _testProposalId = Guid.NewGuid();
    }

    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = new ProposalDescription("Test proposal description")
        };

        _createProposalUseCaseMock
            .Setup(x => x.Execute(request))
            .ReturnsAsync(_testProposalId);

        // Act
        var result = await _controller.Create(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);
        Assert.Equal("ProposalQuery", createdResult.ControllerName);
        Assert.NotNull(createdResult.RouteValues);
        Assert.Equal(_testProposalId, createdResult.RouteValues["id"]);
        
        _createProposalUseCaseMock.Verify(x => x.Execute(request), Times.Once);
    }

    [Fact]
    public async Task Approve_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _approveProposalUseCaseMock
            .Setup(x => x.Execute(_testProposalId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Approve(_testProposalId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _approveProposalUseCaseMock.Verify(x => x.Execute(_testProposalId), Times.Once);
    }

    [Fact]
    public async Task Reject_WithValidIdAndReason_ShouldReturnNoContent()
    {
        // Arrange
        var request = new RejectProposalRequestDto
        {
            Reason = new RejectionReason("Test rejection reason")
        };

        _rejectProposalUseCaseMock
            .Setup(x => x.Execute(_testProposalId, request.Reason))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Reject(_testProposalId, request);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _rejectProposalUseCaseMock.Verify(x => x.Execute(_testProposalId, request.Reason), Times.Once);
    }

    [Fact]
    public async Task Delete_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _deleteProposalUseCaseMock
            .Setup(x => x.Execute(_testProposalId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(_testProposalId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _deleteProposalUseCaseMock.Verify(x => x.Execute(_testProposalId), Times.Once);
    }

    [Fact]
    public async Task Restore_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _restoreProposalUseCaseMock
            .Setup(x => x.Execute(_testProposalId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Restore(_testProposalId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        _restoreProposalUseCaseMock.Verify(x => x.Execute(_testProposalId), Times.Once);
    }

    [Fact]
    public async Task Create_WhenUseCaseThrowsException_ShouldPropagateException()
    {
        // Arrange
        var request = new CreateProposalRequestDto
        {
            Description = new ProposalDescription("Test proposal description")
        };

        var expectedException = new InvalidOperationException("Test exception");
        _createProposalUseCaseMock
            .Setup(x => x.Execute(request))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.Create(request));
        
        Assert.Equal(expectedException, exception);
        _createProposalUseCaseMock.Verify(x => x.Execute(request), Times.Once);
    }

    [Fact]
    public async Task Approve_WhenUseCaseThrowsException_ShouldPropagateException()
    {
        // Arrange
        var expectedException = new InvalidOperationException("Test exception");
        _approveProposalUseCaseMock
            .Setup(x => x.Execute(_testProposalId))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.Approve(_testProposalId));
        
        Assert.Equal(expectedException, exception);
        _approveProposalUseCaseMock.Verify(x => x.Execute(_testProposalId), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullCreateProposalUseCase_ShouldCreateController()
    {
        // Act & Assert - Should not throw when createProposalUseCase is null
        // (Current implementation doesn't validate null parameters)
        var controller = new ProposalCommandController(
            null!,
            _approveProposalUseCaseMock.Object,
            _rejectProposalUseCaseMock.Object,
            _deleteProposalUseCaseMock.Object,
            _restoreProposalUseCaseMock.Object,
            _loggerMock.Object);
        
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldCreateController()
    {
        // Act & Assert - Should not throw when logger is null
        // (Current implementation doesn't validate null parameters)
        var controller = new ProposalCommandController(
            _createProposalUseCaseMock.Object,
            _approveProposalUseCaseMock.Object,
            _rejectProposalUseCaseMock.Object,
            _deleteProposalUseCaseMock.Object,
            _restoreProposalUseCaseMock.Object,
            null!);
        
        Assert.NotNull(controller);
    }
}
