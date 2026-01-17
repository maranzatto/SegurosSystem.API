using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;

namespace PropostaService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly ICreateProposalUseCase _createProposalUseCase;
        private readonly IApproveProposalUseCase _approveProposalUseCase;
        private readonly IRejectProposalUseCase _rejectProposalUseCase;
        private readonly IGetProposalByIdUseCase _getProposalByIdUseCase;
        private readonly IGetAllUseCase _getAllUseCase;
        private readonly IDeleteProposalUseCase _deleteProposalUseCase;
        private readonly IRestoreProposalUseCase _restoreProposalUseCase;
        private readonly ILogger<PropostasController> _logger;
        private readonly IMapper _mapper;

        public PropostasController(ICreateProposalUseCase createProposalUseCase, IApproveProposalUseCase approveProposalUseCase, IRejectProposalUseCase rejectProposalUseCase, IGetProposalByIdUseCase getProposalByIdUseCase, IGetAllUseCase getAllUseCase, IDeleteProposalUseCase deleteProposalUseCase, IRestoreProposalUseCase restoreProposalUseCase, ILogger<PropostasController> logger, IMapper mapper)
        {
            _createProposalUseCase = createProposalUseCase ?? throw new ArgumentNullException(nameof(createProposalUseCase));
            _approveProposalUseCase = approveProposalUseCase ?? throw new ArgumentNullException(nameof(approveProposalUseCase));
            _rejectProposalUseCase = rejectProposalUseCase ?? throw new ArgumentNullException(nameof(rejectProposalUseCase));
            _getProposalByIdUseCase = getProposalByIdUseCase ?? throw new ArgumentNullException(nameof(getProposalByIdUseCase));
            _getAllUseCase = getAllUseCase ?? throw new ArgumentNullException(nameof(getAllUseCase));
            _deleteProposalUseCase = deleteProposalUseCase ?? throw new ArgumentNullException(nameof(deleteProposalUseCase));
            _restoreProposalUseCase = restoreProposalUseCase ?? throw new ArgumentNullException(nameof(restoreProposalUseCase));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProposalRequestDto dto)
        {
            var id = await _createProposalUseCase.Execute(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _approveProposalUseCase.Execute(id);
            return NoContent();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, RejectProposalRequestDto request)
        {
            await _rejectProposalUseCase.Execute(id, request.Reason);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _getProposalByIdUseCase.Execute(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllUseCase.Execute();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteProposalUseCase.Execute(id);
            return NoContent();
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> Restore(Guid id)
        {
            await _restoreProposalUseCase.Execute(id);
            return NoContent();
        }
    }
}
