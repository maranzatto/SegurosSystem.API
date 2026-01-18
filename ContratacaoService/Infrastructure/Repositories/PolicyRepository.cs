using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain.Common;
using ContratacaoService.Infrastructure.Persistence;

namespace ContratacaoService.Infrastructure.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly PolicyDbContext _context;
        private readonly IClock _clock;

        public PolicyRepository(PolicyDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task AddAsync(Policy policy)
        {
            await _context.Policy.AddAsync(policy);
            await _context.SaveChangesAsync();
        }

        public async Task<Policy?> GetByIdAsync(Guid id)
        {
            return await _context.Policy
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }
    }

}
