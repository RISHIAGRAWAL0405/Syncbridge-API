using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Extensions;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StepLinksController : ControllerBase
    {
        private readonly IStepLinkService _service;
        public StepLinksController(IStepLinkService service) => _service = service;

        // POST api/StepLinks/by-step/{taskWorkflowStepId}
        [HttpPost("by-step/{taskWorkflowStepId}")]
        public async Task<ApiResponseData<object>> Create(Guid taskWorkflowStepId, [FromBody] StepLinkCreateRequest request)
        {
            try
            {
                var userId = User.GetUserId();
                return await _service.CreateAsync(taskWorkflowStepId, request, userId);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("Failed to add link", 500); }
        }

        // PUT api/StepLinks/{linkId}
        [HttpPut("{linkId}")]
        public async Task<ApiResponseData<object>> Update(Guid linkId, [FromBody] StepLinkUpdateRequest request)
        {
            try { return await _service.UpdateAsync(linkId, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("Failed to update link", 500); }
        }

        // GET api/StepLinks/by-step/{taskWorkflowStepId}
        [HttpGet("by-step/{taskWorkflowStepId}")]
        public async Task<ApiResponseData<List<StepLinkDto>>> GetByStep(Guid taskWorkflowStepId)
        {
            try { return await _service.GetByStepIdAsync(taskWorkflowStepId); }
            catch (Exception) { return ApiResponseData<List<StepLinkDto>>.Error("Failed to load links", 500); }
        }

        // GET api/StepLinks/by-task/{taskId}
        [HttpGet("by-task/{taskId}")]
        public async Task<ApiResponseData<List<StepLinkDto>>> GetByTask(Guid taskId)
        {
            try { return await _service.GetByTaskIdAsync(taskId); }
            catch (Exception) { return ApiResponseData<List<StepLinkDto>>.Error("Failed to load links", 500); }
        }

        // PATCH api/StepLinks/{linkId}/review
        [HttpPatch("{linkId}/review")]
        public async Task<ApiResponseData<object>> Review(Guid linkId, [FromBody] StepLinkReviewRequest request)
        {
            try
            {
                var userId = User.GetUserId();
                return await _service.ReviewAsync(linkId, request, userId);
            }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (ArgumentException ex) { return ApiResponseData<object>.Error(ex.Message, 400); }
            catch (Exception) { return ApiResponseData<object>.Error("Failed to review link", 500); }
        }

        // DELETE api/StepLinks/{linkId}
        [HttpDelete("{linkId}")]
        public async Task<ApiResponseData<object>> Delete(Guid linkId)
        {
            try { return await _service.DeleteAsync(linkId); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("Failed to delete link", 500); }
        }
    }
}
