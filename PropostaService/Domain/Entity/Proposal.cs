using PropostaService.Domain.Common;
using PropostaService.Domain.Enums;
using System.Runtime.InteropServices;

public class Proposal
{
    private readonly IClock _clock;

    public Guid Id { get; private set; }
    public ProposalStatus Status { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public string? RejectionReason { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    private Proposal()
    {
        Description = string.Empty;
        _clock = null!;
        IsDeleted = false;
    }

    public Proposal(Guid id, string description, IClock clock, ProposalStatus initialStatus = ProposalStatus.UnderReview)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Descrição é obrigatória.");

        _clock = clock;
        Id = id;
        Description = description.Trim();
        Status = initialStatus;
        CreatedAt = UpdatedAt = _clock.UtcNow;
        IsDeleted = false;
    }

    public static Proposal CreateNew(string description, IClock clock)
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

    public void Reject(string reason)
    {
        if (Status != ProposalStatus.UnderReview)
            throw new InvalidOperationException($"Proposta no status {Status} não pode ser rejeitada.");

        if (IsDeleted)
            throw new InvalidOperationException("Cannot reject a deleted proposal.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Motivo da rejeição é obrigatório.");

        Status = ProposalStatus.Rejected;
        RejectionReason = reason.Trim();
        UpdatedAt = _clock.UtcNow;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Descrição inválida.");

        if (IsDeleted)
            throw new InvalidOperationException("Cannot update a deleted proposal.");

        if (Status != ProposalStatus.UnderReview)
            throw new InvalidOperationException("Só pode editar descrição enquanto em análise.");

        Description = newDescription.Trim();
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
        UpdatedAt = _clock.UtcNow;
    }
}