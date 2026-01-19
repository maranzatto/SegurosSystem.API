namespace ContratacaoService.Application.DTOs
{
    public class PolicyResponseDto
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public string? ProposalName { get; set; }
        public string PolicyNumber { get; set; } = default!;
        public DateTime ContractedAt { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; } = default!;
    }
}
