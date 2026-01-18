using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Interfaces;

namespace ContratacaoService.Api.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PolicyCommandController : ControllerBase
    {
        private readonly IContractPolicyUseCase _contractPolicyUseCase;
        private readonly ILogger<PolicyCommandController> _logger;

        public PolicyCommandController(
            IContractPolicyUseCase contractPolicyUseCase,
            ILogger<PolicyCommandController> logger)
        {
            _contractPolicyUseCase = contractPolicyUseCase;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Contract([FromBody] ContractPolicyRequestDto dto)
        {
            var policyId = await _contractPolicyUseCase.ExecuteAsync(dto.ProposalId);

            return CreatedAtAction(
                nameof(PolicyQueryController.GetById),
                "PolicyQuery",
                new { id = policyId },
                null);
        }
    }
}
