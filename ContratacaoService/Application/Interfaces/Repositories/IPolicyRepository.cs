using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Application.Interfaces.Repositories
{
    public interface IPolicyRepository
    {
        Task AddAsync(Policy policy);
        Task<Policy?> GetByIdAsync(Guid id);
        Task<IEnumerable<Policy>> GetAllAsync();
        Task UpdateAsync(Policy policy);
        Task DeleteAsync(Guid id);
        Task RestoreAsync(Guid id);
    }

}