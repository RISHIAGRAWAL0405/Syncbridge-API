using PhotographyPortfolioAPI.Contracts.Common;
using PhotographyPortfolioAPI.Contracts.DTOs.Project;

namespace PhotographyPortfolioAPI.Interfaces.Project
{
    public interface IClientService
    {
        Task<ApiResponseData<object>> CreateAsync(ClientCreateRequest request);
        Task<ApiResponseData<object>> UpdateAsync(Guid clientId, ClientUpdateRequest request);
        Task<ApiResponseData<ClientDto>> GetByIdAsync(Guid clientId);
        Task<ApiResponseData<List<ClientDto>>> GetAllAsync();
        Task<ApiResponseData<List<ClientDto>>> GetAllActiveAsync();
        Task<ApiResponseData<object>> ActiveInactiveAsync(Guid clientId, bool isActive);
    }
}
