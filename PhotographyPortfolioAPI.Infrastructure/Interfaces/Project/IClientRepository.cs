using PhotographyPortfolioAPI.Contracts.DTOs.Project;
using PhotographyPortfolioAPI.Contracts.Models.Project;

namespace PhotographyPortfolioAPI.Infrastructure.Interfaces.Project
{
    public interface IClientRepository
    {
        Task<Guid> CreateAsync(ClientMaster client);
        Task UpdateAsync(ClientMaster client);
        Task<ClientMaster?> GetByIdAsync(Guid clientId);
        Task<List<ClientDto>> GetAllAsync();
        Task<List<ClientDto>> GetAllActiveAsync();
        Task ActiveInactiveAsync(Guid clientId, bool isActive);
    }
}
