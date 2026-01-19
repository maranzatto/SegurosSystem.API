using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ContratacaoService.Api.Controllers
{
    [ApiController]
    [Route("api/policies")]
    public class PolicyQueryController : ControllerBase
    {
        private readonly IGetPolicyByIdUseCase _getByIdUseCase;
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly ILogger<PolicyQueryController> _logger;

        public PolicyQueryController(IGetPolicyByIdUseCase getByIdUseCase, IGetAllUseCase getAllUseCase, ILogger<PolicyQueryController> logger)
        {
            _getByIdUseCase = getByIdUseCase ?? throw new ArgumentNullException(nameof(getByIdUseCase));
            _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var policy = await _getByIdUseCase.ExecuteAsync(id);
            return Ok(policy);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllUseCase.Execute();
            return Ok(result);
        }
    }
}
