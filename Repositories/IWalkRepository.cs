using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null);
        
        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid Id, Walk walk);

        Task<Walk?> DeleteAsync(Guid Id);
    }
}
