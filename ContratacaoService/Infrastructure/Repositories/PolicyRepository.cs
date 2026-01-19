using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Common;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.Exceptions;
using ContratacaoService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContratacaoService.Infrastructure.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly PolicyDbContext _context;
        private readonly IClock _clock;

        public PolicyRepository(PolicyDbContext context, IClock clock)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _clock = clock ?? throw new ArgumentNullException(nameof(clock));
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
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            return await _context.Policy
                .Where(p => !p.IsDeleted && p.Status == PolicyStatus.Active)
                .OrderBy(p => p.ProposalName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateAsync(Policy Policy)
        {
            _context.Policy.Update(Policy);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var Policy = await _context.Policy
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Policy == null)
            {
                throw new DomainException("Proposta não encontrada.");
            }

            Policy.Delete();

            _context.Policy.Update(Policy);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid id)
        {
            var Policy = await _context.Policy
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Policy == null)
            {
                throw new DomainException("Proposta não encontrada.");
            }

            Policy.Restore();

            _context.Policy.Update(Policy);
            await _context.SaveChangesAsync();
        }
    }

}
