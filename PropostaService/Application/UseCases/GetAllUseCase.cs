using AutoMapper;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;
using PropostaService.Application.Interfaces.Repositories;
using PropostaService.Domain.Exceptions;

namespace PropostaService.Application.UseCases
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IProposalRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUseCase(IProposalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProposalResponseDto>> Execute()
        {
            var proposals = await _repository.GetAllAsync() 
                ?? throw new DomainException("Erro ao listar propostas");

            return _mapper.Map<IEnumerable<ProposalResponseDto>>(proposals);
        }
    }
    
}
