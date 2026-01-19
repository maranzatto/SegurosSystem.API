using AutoMapper;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Exceptions;

namespace ContratacaoService.Application.UseCases
{
    public class GetAllUseCase : IGetAllUseCase
    {
        private readonly IPolicyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUseCase(IPolicyRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<PolicyResponseDto>> Execute()
        {
            var proposals = await _repository.GetAllAsync()
                ?? throw new DomainException("Erro ao listar contratos");

            return _mapper.Map<IEnumerable<PolicyResponseDto>>(proposals);
        }
    }
}
