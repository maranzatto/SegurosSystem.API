using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Api.Controllers;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace ContratacaoService.Tests.Api.Controllers;

public class PolicyCommandControllerTests
{
    private readonly Mock<IContractPolicyUseCase> _contractPolicyUseCaseMock;
    private readonly Mock<ILogger<PolicyCommandController>> _loggerMock;
    private readonly PolicyCommandController _controller;
    private readonly Guid _testPolicyId;

    public PolicyCommandControllerTests()
    {
        _contractPolicyUseCaseMock = new Mock<IContractPolicyUseCase>();
        _loggerMock = new Mock<ILogger<PolicyCommandController>>();
        _controller = new PolicyCommandController(
            _contractPolicyUseCaseMock.Object,
            _loggerMock.Object);
        
        _testPolicyId = Guid.NewGuid();
    }

    [Fact]
    public async Task Contract_WithValidRequest_ShouldReturnCreatedResult()
    {
        var request = new ContractPolicyRequestDto
        {
            ProposalId = Guid.NewGuid()
        };

        _contractPolicyUseCaseMock
            .Setup(x => x.ExecuteAsync(request.ProposalId))
            .ReturnsAsync(_testPolicyId);

        var result = await _controller.Contract(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetById", createdResult.ActionName);
        Assert.Equal("PolicyQuery", createdResult.ControllerName);
        Assert.NotNull(createdResult.RouteValues);
        Assert.Equal(_testPolicyId, createdResult.RouteValues["id"]);
        
        _contractPolicyUseCaseMock.Verify(x => x.ExecuteAsync(request.ProposalId), Times.Once);
    }

    [Fact]
    public async Task Contract_WhenUseCaseThrowsException_ShouldPropagateException()
    {
        var request = new ContractPolicyRequestDto
        {
            ProposalId = Guid.NewGuid()
        };

        var expectedException = new InvalidOperationException("Test exception");
        _contractPolicyUseCaseMock
            .Setup(x => x.ExecuteAsync(request.ProposalId))
            .ThrowsAsync(expectedException);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _controller.Contract(request));
        
        Assert.Equal(expectedException, exception);
        _contractPolicyUseCaseMock.Verify(x => x.ExecuteAsync(request.ProposalId), Times.Once);
    }

    [Fact]
    public async Task Contract_WithNullProposalId_ShouldStillCallUseCase()
    {
        var request = new ContractPolicyRequestDto
        {
            ProposalId = Guid.Empty
        };

        _contractPolicyUseCaseMock
            .Setup(x => x.ExecuteAsync(request.ProposalId))
            .ReturnsAsync(_testPolicyId);

        var result = await _controller.Contract(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        _contractPolicyUseCaseMock.Verify(x => x.ExecuteAsync(request.ProposalId), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullContractPolicyUseCase_ShouldCreateController()
    {
        var controller = new PolicyCommandController(
            null!,
            _loggerMock.Object);
        
        Assert.NotNull(controller);
    }

    [Fact]
    public void Constructor_WithNullLogger_ShouldCreateController()
    {
        var controller = new PolicyCommandController(
            _contractPolicyUseCaseMock.Object,
            null!);
        
        Assert.NotNull(controller);
    }
}
