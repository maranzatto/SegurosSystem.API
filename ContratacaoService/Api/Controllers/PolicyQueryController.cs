using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.Interfaces;

namespace ContratacaoService.Api.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PolicyQueryController : ControllerBase
    {
        private readonly IGetPolicyByIdUseCase _getByIdUseCase;
        private readonly ILogger<PolicyQueryController> _logger;

        public PolicyQueryController(
            IGetPolicyByIdUseCase getByIdUseCase,
            ILogger<PolicyQueryController> logger)
        {
            _getByIdUseCase = getByIdUseCase;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var policy = await _getByIdUseCase.ExecuteAsync(id);
            return Ok(policy);
        }
    }
}
