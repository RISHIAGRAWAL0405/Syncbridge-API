using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Interfaces.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhotographyPortfolioAPI.Controllers.Project
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;

        public ClientsController(IClientService service) => _service = service;

        [HttpPost]
        public async Task<ApiResponseData<object>> Create([FromBody] ClientCreateRequest request)
        {
            try { return await _service.CreateAsync(request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while creating client", 500); }
        }

        [HttpPut("{client_id}")]
        public async Task<ApiResponseData<object>> Update(Guid client_id, [FromBody] ClientUpdateRequest request)
        {
            try { return await _service.UpdateAsync(client_id, request); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred while updating client", 500); }
        }

        [HttpGet("{client_id}")]
        public async Task<ApiResponseData<ClientDto>> GetById(Guid client_id)
        {
            try { return await _service.GetByIdAsync(client_id); }
            catch (KeyNotFoundException ex) { return ApiResponseData<ClientDto>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<ClientDto>.Error("An error occurred", 500); }
        }

        [HttpGet]
        public async Task<ApiResponseData<List<ClientDto>>> GetAll()
        {
            try { return await _service.GetAllAsync(); }
            catch (Exception) { return ApiResponseData<List<ClientDto>>.Error("An error occurred", 500); }
        }

        [HttpGet("active")]
        public async Task<ApiResponseData<List<ClientDto>>> GetAllActive()
        {
            try { return await _service.GetAllActiveAsync(); }
            catch (Exception) { return ApiResponseData<List<ClientDto>>.Error("An error occurred", 500); }
        }

        [HttpPatch("{client_id}/status")]
        public async Task<ApiResponseData<object>> ActiveInactive(Guid client_id, [FromQuery] bool is_active)
        {
            try { return await _service.ActiveInactiveAsync(client_id, is_active); }
            catch (KeyNotFoundException ex) { return ApiResponseData<object>.Error(ex.Message, 404); }
            catch (Exception) { return ApiResponseData<object>.Error("An error occurred", 500); }
        }
    }
}
