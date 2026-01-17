using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.Interfaces;

namespace PropostaService.Api.Controllers
{
    [ApiController]
    [Route("api/proposals")]
    public class ProposalQueryController : ControllerBase
    {
        private readonly IGetProposalByIdUseCase _getByIdUseCase;
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly ILogger<ProposalQueryController> _logger;
        public ProposalQueryController(
            IGetProposalByIdUseCase getByIdUseCase,
            IGetAllUseCase getAllUseCase,
            ILogger<ProposalQueryController> logger)
        {
            _getByIdUseCase = getByIdUseCase;
            _getAllUseCase = getAllUseCase;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllUseCase.Execute();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _getByIdUseCase.Execute(id);
            return Ok(result);
        }
    }
}
