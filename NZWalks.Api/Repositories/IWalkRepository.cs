using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null
            ,string? sortBy = null, bool isAcending = true
            ,int pageNumber = 1, int pageSize = 10);
        
        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid Id, Walk walk);

        Task<Walk?> DeleteAsync(Guid Id);
    }
}
