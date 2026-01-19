using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Domain.Exceptions;

namespace ContratacaoService.Application.UseCases
{
    public class DeletePolicyUseCase : IDeletePolicyUseCase
    {
        private readonly IPolicyRepository _repository;

        public DeletePolicyUseCase(IPolicyRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Execute(Guid id)
        {
            var policy = await _repository.GetByIdAsync(id)
                ?? throw new DomainException("Proposta não encontrada.");

            policy.Delete();

            await _repository.UpdateAsync(policy);
        }
    }
}
