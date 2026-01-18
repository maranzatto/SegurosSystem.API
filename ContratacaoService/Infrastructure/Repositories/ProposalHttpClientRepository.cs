using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces.Repositories;

namespace ContratacaoService.Infrastructure.Repositories
{
    public class ProposalHttpClient : IProposalHttpClient
    {
        private readonly HttpClient _http;

        public ProposalHttpClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ProposalDto> GetByIdAsync(Guid proposalId)
        {
            var response = await _http.GetAsync($"/api/proposals/{proposalId}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProposalDto>()
                   ?? throw new ApplicationException("Resposta inválida do PropostaService.");
        }
    }
}
