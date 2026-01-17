namespace PropostaService.Infrastructure.Repositories
{
    public interface IProposalRepository
    {
        Task AddAsync(Proposal proposal);
        Task<Proposal?> GetByIdAsync(Guid id);
        Task<IEnumerable<Proposal>> GetAllAsync();
        Task UpdateAsync(Proposal proposal);
        Task DeleteAsync(Guid id);
        Task RestoreAsync(Guid id);
    }

}
