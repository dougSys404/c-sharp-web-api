using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> getAllAsync();
        Task<Walk>  CreateAsync(Walk walk);

        Task<Walk?> getByIdAsync(Guid id);

        
    }
}
