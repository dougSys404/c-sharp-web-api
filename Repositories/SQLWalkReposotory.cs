﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Walk?> UpdateAsync(Guid Id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if ( existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthKm = walk.LengthKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return existingWalk;

        }

        public async Task<Walk?> DeleteAsync(Guid Id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingWalk == null)
            {
                return null;
            }

            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
