using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.ValueObjects;
using ContratacaoService.Domain.Common;
using ContratacaoService.Domain.Exceptions;
using Moq;

namespace ContratacaoService.Tests.Domain.Entities;

public class PolicyTests
{
    private readonly Mock<IClock> _clockMock;
    private readonly DateTime _fixedDateTime;

    public PolicyTests()
    {
        _clockMock = new Mock<IClock>();
        _fixedDateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        _clockMock.Setup(x => x.UtcNow).Returns(_fixedDateTime);
    }

    [Fact]
    public void Create_WithApprovedProposal_ShouldCreatePolicySuccessfully()
    {
        var proposalId = Guid.NewGuid();
        var proposalName = "Test Proposal";
        var proposalStatus = ProposalStatusContract.Approved;

        var policy = Policy.Create(proposalId, proposalStatus, proposalName, _clockMock.Object);

        Assert.NotNull(policy);
        Assert.NotEqual(Guid.Empty, policy.Id);
        Assert.Equal(proposalId, policy.ProposalId);
        Assert.Equal(proposalName, policy.ProposalName);
        Assert.Equal(PolicyStatus.Active, policy.Status);
        Assert.Equal(_fixedDateTime, policy.ContractedAt);
        Assert.False(policy.IsDeleted);
        Assert.NotNull(policy.PolicyNumber);
        Assert.NotNull(policy.Period);
    }

    [Fact]
    public void Create_WithNonApprovedProposal_ShouldThrowDomainException()
    {
        var proposalId = Guid.NewGuid();
        var proposalName = "Test Proposal";
        var proposalStatus = ProposalStatusContract.UnderReview;

        var exception = Assert.Throws<DomainException>(() => 
            Policy.Create(proposalId, proposalStatus, proposalName, _clockMock.Object));
        
        Assert.Equal("Proposta não aprovada não pode ser contratada.", exception.Message);
    }

    [Fact]
    public void Cancel_WithActivePolicy_ShouldCancelPolicy()
    {
        var policy = CreateTestPolicy();

        policy.Cancel();

        Assert.Equal(PolicyStatus.Canceled, policy.Status);
    }

    [Fact]
    public void Cancel_WithNonActivePolicy_ShouldThrowDomainException()
    {
        var policy = CreateTestPolicy();
        policy.Cancel();

        var exception = Assert.Throws<DomainException>(() => policy.Cancel());
        Assert.Equal("Apenas apólices ativas podem ser canceladas.", exception.Message);
    }

    [Fact]
    public void Delete_WithNonDeletedPolicy_ShouldMarkAsDeleted()
    {
        var policy = CreateTestPolicy();

        policy.Delete();

        Assert.True(policy.IsDeleted);
    }

    [Fact]
    public void Delete_WithAlreadyDeletedPolicy_ShouldThrowDomainException()
    {
        var policy = CreateTestPolicy();
        policy.Delete();

        var exception = Assert.Throws<DomainException>(() => policy.Delete());
        Assert.Equal("Apólice já excluída.", exception.Message);
    }

    [Fact]
    public void Restore_WithDeletedPolicy_ShouldRestorePolicy()
    {
        var policy = CreateTestPolicy();
        policy.Delete();

        policy.Restore();

        Assert.False(policy.IsDeleted);
    }

    [Fact]
    public void Restore_WithNonDeletedPolicy_ShouldThrowDomainException()
    {
        var policy = CreateTestPolicy();

        var exception = Assert.Throws<DomainException>(() => policy.Restore());
        Assert.Equal("Apólice não está excluída.", exception.Message);
    }

    private Policy CreateTestPolicy()
    {
        return Policy.Create(
            Guid.NewGuid(),
            ProposalStatusContract.Approved,
            "Test Proposal",
            _clockMock.Object);
    }
}
