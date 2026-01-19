using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Exceptions;

namespace ContratacaoService.Application.UseCases
{
    public class RestorePolicyUseCase : IRestorePolicyUseCase
    {
        private readonly IPolicyRepository _repository;

        public RestorePolicyUseCase(IPolicyRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Execute(Guid id)
        {
            var policy = await _repository.GetByIdAsync(id)
                ?? throw new DomainException("Contrato não encontrado.");

            policy.Restore();

            await _repository.UpdateAsync(policy);
        }
    }
}
