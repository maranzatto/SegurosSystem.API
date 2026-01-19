using PropostaService.Domain.Common;
using PropostaService.Domain.Enums;
using PropostaService.Domain.ValueObjects;

public class Proposal
{
    private readonly IClock _clock;

    public Guid Id { get; private set; }
    public ProposalStatus Status { get; private set; }
    public ProposalDescription Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public RejectionReason? RejectionReason { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    private Proposal()
    {
        Description = null!;
        _clock = null!;
        IsDeleted = false;
    }

    public Proposal(Guid id, ProposalDescription description, IClock clock, ProposalStatus initialStatus = ProposalStatus.UnderReview)
    {
        _clock = clock;
        Id = id;
        Description = description;
        Status = initialStatus;
        CreatedAt = UpdatedAt = _clock.UtcNow;
        IsDeleted = false;
    }

    public static Proposal CreateNew(ProposalDescription description, IClock clock)
    {
        return new Proposal(Guid.NewGuid(), description, clock, ProposalStatus.UnderReview);
    }

    public void SetClock(IClock clock)
    {
        if (_clock == null)
        {
            typeof(Proposal)
                .GetField("_clock", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(this, clock);
        }
    }

    public void Approve()
    {
        if (Status != ProposalStatus.UnderReview)
            throw new InvalidOperationException($"Proposta no status {Status} não pode ser aprovada.");

        if (IsDeleted)
            throw new InvalidOperationException("Cannot approve a deleted proposal.");

        Status = ProposalStatus.Approved;
        UpdatedAt = _clock.UtcNow;
    }

    public void Reject(RejectionReason reason)
    {
        if (Status != ProposalStatus.UnderReview)
            throw new InvalidOperationException($"Proposta no status {Status} não pode ser rejeitada.");

        if (IsDeleted)
            throw new InvalidOperationException("Cannot reject a deleted proposal.");

        Status = ProposalStatus.Rejected;
        RejectionReason = reason;
        UpdatedAt = _clock.UtcNow;

    }

    public void UpdateDescription(ProposalDescription newDescription)
    {
        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted proposal.");
        if (Status != ProposalStatus.UnderReview)
            throw new InvalidOperationException("Só pode editar descrição enquanto em análise.");

        Description = newDescription;
        UpdatedAt = _clock.UtcNow;
    }

    public void Delete()
    {
        if (IsDeleted)
            throw new InvalidOperationException("Proposal is already deleted.");

        IsDeleted = true;
        UpdatedAt = _clock.UtcNow;
    }

    public void Restore()
    {
        if (!IsDeleted)
            throw new InvalidOperationException("Proposal is not deleted.");

        IsDeleted = false;
        Status = ProposalStatus.UnderReview;
        UpdatedAt = _clock.UtcNow;
    }
}