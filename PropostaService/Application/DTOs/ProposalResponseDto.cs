namespace PropostaService.Application.DTOs
{
    public class ProposalResponseDto
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
