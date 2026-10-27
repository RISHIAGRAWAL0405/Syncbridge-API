using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;
using PhotographyPortfolioAPI.Infrastructure.Interfaces.Project;
using PhotographyPortfolioAPI.Interfaces.Project;

namespace PhotographyPortfolioAPI.Services.Project
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;

        public ClientService(IClientRepository repo) => _repo = repo;

        public async Task<ApiResponseData<object>> CreateAsync(ClientCreateRequest request)
        {
            var existing = await _repo.GetByIdAsync(request.client_id);
            if (existing != null)
                throw new InvalidOperationException("A client with this user ID already exists");

            var client = new ClientMaster
            {
                client_id = request.client_id,   // user_id from UI
                client_name = request.client_name,
                email = request.email,
                mobile = request.mobile,
                company_name = request.company_name,
                address = request.address,
                is_active = true,
                created_at = DateTime.UtcNow
            };
            var id = await _repo.CreateAsync(client);
            return ApiResponseData<object>.Success(null, "Client created successfully", id);
        }

        public async Task<ApiResponseData<object>> UpdateAsync(Guid clientId, ClientUpdateRequest request)
        {
            var client = await _repo.GetByIdAsync(clientId);
            if (client == null) throw new KeyNotFoundException("Client not found");

            if (!string.IsNullOrWhiteSpace(request.client_name)) client.client_name = request.client_name;
            if (!string.IsNullOrWhiteSpace(request.email)) client.email = request.email;
            if (!string.IsNullOrWhiteSpace(request.mobile)) client.mobile = request.mobile;
            if (!string.IsNullOrWhiteSpace(request.company_name)) client.company_name = request.company_name;
            if (!string.IsNullOrWhiteSpace(request.address)) client.address = request.address;
            client.updated_at = DateTime.UtcNow;

            await _repo.UpdateAsync(client);
            return ApiResponseData<object>.Success(null, "Client updated successfully", clientId);
        }

        public async Task<ApiResponseData<ClientDto>> GetByIdAsync(Guid clientId)
        {
            var client = await _repo.GetByIdAsync(clientId);
            if (client == null) throw new KeyNotFoundException("Client not found");
            var dto = new ClientDto
            {
                client_id = client.client_id,
                client_name = client.client_name,
                email = client.email,
                mobile = client.mobile,
                company_name = client.company_name,
                address = client.address,
                is_active = client.is_active,
                created_at = client.created_at
            };
            return ApiResponseData<ClientDto>.Success(dto, "Client retrieved successfully");
        }

        public async Task<ApiResponseData<List<ClientDto>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return ApiResponseData<List<ClientDto>>.Success(list, "Clients retrieved successfully");
        }

        public async Task<ApiResponseData<List<ClientDto>>> GetAllActiveAsync()
        {
            var list = await _repo.GetAllActiveAsync();
            return ApiResponseData<List<ClientDto>>.Success(list, "Clients retrieved successfully");
        }

        public async Task<ApiResponseData<object>> ActiveInactiveAsync(Guid clientId, bool isActive)
        {
            var client = await _repo.GetByIdAsync(clientId);
            if (client == null) throw new KeyNotFoundException("Client not found");
            await _repo.ActiveInactiveAsync(clientId, isActive);
            return ApiResponseData<object>.Success(new { id = clientId }, "Client status updated successfully");
        }
    }
}
