using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.DTOs;
using PropostaService.Application.Interfaces;

namespace PropostaService.Api.Controllers
{
    [ApiController]
    [Route("api/proposals")]
    public class ProposalCommandController : ControllerBase
    {
        private readonly ICreateProposalUseCase _createProposalUseCase;
        private readonly IApproveProposalUseCase _approveProposalUseCase;
        private readonly IRejectProposalUseCase _rejectProposalUseCase;
        private readonly IDeleteProposalUseCase _deleteProposalUseCase;
        private readonly IRestoreProposalUseCase _restoreProposalUseCase;
        private readonly ILogger<ProposalCommandController> _logger;
        public ProposalCommandController(
            ICreateProposalUseCase createProposalUseCase,
            IApproveProposalUseCase approveProposalUseCase,
            IRejectProposalUseCase rejectProposalUseCase,
            IDeleteProposalUseCase deleteProposalUseCase,
            IRestoreProposalUseCase restoreProposalUseCase,
            ILogger<ProposalCommandController> logger)
        {
            _createProposalUseCase = createProposalUseCase;
            _approveProposalUseCase = approveProposalUseCase;
            _rejectProposalUseCase = rejectProposalUseCase;
            _deleteProposalUseCase = deleteProposalUseCase;
            _restoreProposalUseCase = restoreProposalUseCase;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProposalRequestDto dto)
        {
            var id = await _createProposalUseCase.Execute(dto);
            return CreatedAtAction(nameof(ProposalQueryController.GetById),
                "ProposalQuery", new { id }, null);
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