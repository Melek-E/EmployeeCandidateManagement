using ApiCandidate = EmployeeCandidateManagement.API.Models.Candidate;
using DomainCandidate = EmployeeManagement.Core.Entities.Candidate;
using EmployeeManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EmployeeCandidateManagement.API.Models;

namespace EmployeeCandidateManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService ?? throw new ArgumentNullException(nameof(candidateService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DomainCandidate>>> GetCandidates()
        {
            var candidates = await _candidateService.GetAllCandidatesAsync();
            return Ok(candidates);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DomainCandidate>> GetCandidate(int id)
        {
            var candidate = await _candidateService.GetCandidateByIdAsync(id);

            if (candidate == null)
                return NotFound();

            return Ok(candidate);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult<DomainCandidate>> CreateCandidate(DomainCandidate candidate)
        {
            try
            {
                var createdCandidate = await _candidateService.CreateCandidateAsync(candidate);
                return CreatedAtAction(nameof(GetCandidate), new { id = createdCandidate.Id }, createdCandidate);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<IActionResult> UpdateCandidate(int id, DomainCandidate candidate)
        {
            if (id != candidate.Id)
                return BadRequest();

            try
            {
                await _candidateService.UpdateCandidateAsync(candidate);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            try
            {
                await _candidateService.DeleteCandidateAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/process-resume")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult<DomainCandidate>> ProcessCandidateResume(int id, [FromBody] ResumeProcessRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ResumeContent))
                return BadRequest(new { message = "Resume content is required." });

            var candidate = await _candidateService.ProcessCandidateResumeAsync(id, request.ResumeContent);

            if (candidate == null)
                return NotFound();

            return Ok(candidate);
        }

        [HttpPost("{id}/evaluate")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult<decimal>> EvaluateCandidate(int id, [FromBody] EvaluateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.JobRequirements))
                return BadRequest(new { message = "Job requirements are required." });

            try
            {
                var matchPercentage = await _candidateService.EvaluateCandidateMatchAsync(id, request.JobRequirements);
                return Ok(new { matchPercentage });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/suggest-salary")]
        [Authorize(Roles = "Admin,Recruiter")]
        public async Task<ActionResult<SalarySuggestionResponse>> SuggestSalary(int id, [FromBody] SalarySuggestionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Position))
                return BadRequest(new { message = "Position is required." });

            try
            {
                var (minSalary, maxSalary) = await _candidateService.SuggestSalaryRangeAsync(id, request.Position);
                return Ok(new SalarySuggestionResponse
                {
                    MinSalary = minSalary,
                    MaxSalary = maxSalary,
                    Position = request.Position
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
