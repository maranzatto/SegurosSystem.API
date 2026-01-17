using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Common;
using PropostaService.Domain.Exceptions;
using PropostaService.Infrastructure.Persistence;

namespace PropostaService.Infrastructure.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly ProposalDbContext _context;
        private readonly IClock _clock;

        public ProposalRepository(ProposalDbContext context, IClock clock)
        {
            _context = context;
            _clock = clock;
        }

        public async Task AddAsync(Proposal proposal)
        {
            await _context.Proposals.AddAsync(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task<Proposal?> GetByIdAsync(Guid id)
        {
            var proposal = await _context.Proposals.FirstOrDefaultAsync(x => x.Id == id);

            if (proposal != null)
            {
                proposal.SetClock(_clock);
            }

            return proposal;
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync()
        {
            return await _context.Proposals.Where(p => !p.IsDeleted).AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Proposal proposal)
        {
            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var proposal = await _context.Proposals
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
            {
                throw new DomainException("Proposta não encontrada.");
            }

            proposal.Delete();

            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task RestoreAsync(Guid id)
        {
            var proposal = await _context.Proposals
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proposal == null)
            {
                throw new DomainException("Proposta não encontrada.");
            }

            proposal.Restore();

            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
        }
    }
}
