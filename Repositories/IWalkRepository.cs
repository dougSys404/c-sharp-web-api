using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk>  CreateAsync(Walk walk);
    }
}
