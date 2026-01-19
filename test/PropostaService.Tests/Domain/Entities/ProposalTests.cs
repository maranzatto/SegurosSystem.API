using PropostaService.Domain.Common;
using PropostaService.Domain.Enums;
using PropostaService.Domain.ValueObjects;
using Moq;

namespace PropostaService.Tests.Domain.Entities;

public class ProposalTests
{
    private readonly Mock<IClock> _clockMock;
    private readonly DateTime _fixedDateTime;

    public ProposalTests()
    {
        _clockMock = new Mock<IClock>();
        _fixedDateTime = new DateTime(2024, 1, 1, 12, 0, 0);
        _clockMock.Setup(x => x.UtcNow).Returns(_fixedDateTime);
    }

    [Fact]
    public void CreateNew_ShouldCreateProposalWithUnderReviewStatus()
    {
        var description = new ProposalDescription("Test proposal description");

        var proposal = Proposal.CreateNew(description, _clockMock.Object);

        Assert.NotNull(proposal);
        Assert.NotEqual(Guid.Empty, proposal.Id);
        Assert.Equal(ProposalStatus.UnderReview, proposal.Status);
        Assert.Equal(description, proposal.Description);
        Assert.Equal(_fixedDateTime, proposal.CreatedAt);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
        Assert.Null(proposal.RejectionReason);
        Assert.False(proposal.IsDeleted);
    }

    [Fact]
    public void Approve_WithUnderReviewStatus_ShouldApproveProposal()
    {
        var proposal = CreateTestProposal();

        proposal.Approve();

        Assert.Equal(ProposalStatus.Approved, proposal.Status);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
    }

    [Fact]
    public void Approve_WithNonUnderReviewStatus_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Approve();

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Approve());
        Assert.Contains("não pode ser aprovada", exception.Message);
    }

    [Fact]
    public void Approve_WithDeletedProposal_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Delete();

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Approve());
        Assert.Equal("Cannot approve a deleted proposal.", exception.Message);
    }

    [Fact]
    public void Reject_WithUnderReviewStatus_ShouldRejectProposal()
    {
        var proposal = CreateTestProposal();
        var rejectionReason = new RejectionReason("Insufficient documentation");

        proposal.Reject(rejectionReason);

        Assert.Equal(ProposalStatus.Rejected, proposal.Status);
        Assert.Equal(rejectionReason, proposal.RejectionReason);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
    }

    [Fact]
    public void Reject_WithNonUnderReviewStatus_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Approve();
        var rejectionReason = new RejectionReason("Test reason");

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Reject(rejectionReason));
        Assert.Contains("não pode ser rejeitada", exception.Message);
    }

    [Fact]
    public void Reject_WithDeletedProposal_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Delete();
        var rejectionReason = new RejectionReason("Test reason");

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Reject(rejectionReason));
        Assert.Equal("Cannot reject a deleted proposal.", exception.Message);
    }

    [Fact]
    public void UpdateDescription_WithUnderReviewStatus_ShouldUpdateDescription()
    {
        var proposal = CreateTestProposal();
        var newDescription = new ProposalDescription("Updated description");

        proposal.UpdateDescription(newDescription);

        Assert.Equal(newDescription, proposal.Description);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
    }

    [Fact]
    public void UpdateDescription_WithDeletedProposal_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Delete();
        var newDescription = new ProposalDescription("Updated description");

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.UpdateDescription(newDescription));
        Assert.Equal("Cannot update a deleted proposal.", exception.Message);
    }

    [Fact]
    public void UpdateDescription_WithNonUnderReviewStatus_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Approve();
        var newDescription = new ProposalDescription("Updated description");

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.UpdateDescription(newDescription));
        Assert.Equal("Só pode editar descrição enquanto em análise.", exception.Message);
    }

    [Fact]
    public void Delete_WithNonDeletedProposal_ShouldMarkAsDeleted()
    {
        var proposal = CreateTestProposal();

        proposal.Delete();

        Assert.True(proposal.IsDeleted);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
    }

    [Fact]
    public void Delete_WithAlreadyDeletedProposal_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();
        proposal.Delete();

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Delete());
        Assert.Equal("Proposal is already deleted.", exception.Message);
    }

    [Fact]
    public void Restore_WithDeletedProposal_ShouldRestoreProposal()
    {
        var proposal = CreateTestProposal();
        proposal.Delete();

        proposal.Restore();

        Assert.False(proposal.IsDeleted);
        Assert.Equal(ProposalStatus.UnderReview, proposal.Status);
        Assert.Equal(_fixedDateTime, proposal.UpdatedAt);
    }

    [Fact]
    public void Restore_WithNonDeletedProposal_ShouldThrowInvalidOperationException()
    {
        var proposal = CreateTestProposal();

        var exception = Assert.Throws<InvalidOperationException>(() => proposal.Restore());
        Assert.Equal("Proposal is not deleted.", exception.Message);
    }

    private Proposal CreateTestProposal()
    {
        return Proposal.CreateNew(
            new ProposalDescription("Test proposal description"),
            _clockMock.Object);
    }
}
