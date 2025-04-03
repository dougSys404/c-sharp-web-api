using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Repositories
{
    public class SQLWalkReposotory : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkReposotory(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        async Task<List<Walk>> IWalkRepository.getAllAsync()
        {
            return await dbContext.Walks.ToListAsync();
        }
    }
}
