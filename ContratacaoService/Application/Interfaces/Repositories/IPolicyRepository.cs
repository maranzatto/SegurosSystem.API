using ContratacaoService.Domain.Entities;

public interface IPolicyRepository
{
    Task AddAsync(Policy policy);
    Task<Policy?> GetByIdAsync(Guid id);
}
