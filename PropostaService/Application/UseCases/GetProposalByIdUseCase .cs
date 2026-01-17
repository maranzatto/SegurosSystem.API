using AutoMapper;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Application.Interfaces.Repositories;
using PropostaService.Domain.Exceptions;

namespace PropostaService.Application.UseCases
{
    public class GetProposalByIdUseCase : IGetProposalByIdUseCase
    {
        private readonly IProposalRepository _repository;
        private readonly IMapper _mapper;

        public GetProposalByIdUseCase(IProposalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProposalResponseDto> Execute(Guid id)
        {
            var proposal = await _repository.GetByIdAsync(id)
                ?? throw new DomainException("Proposta não encontrada.");

            return _mapper.Map<ProposalResponseDto>(proposal);
        }
    }
}
