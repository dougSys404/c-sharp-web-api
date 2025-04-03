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
 
        public async Task<List<Walk>> getAllAsync()
        {
            return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> getByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }
    }
}
