using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;

namespace NZWalks.Api.Controllers
{
    // https://localhost:7121/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // getting all regions using list
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            // getting data from database - domain model
            var regionsDomain = await dbContext.Regions.ToListAsync();

            var regionsDto = new List<RegionsDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionsDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl

                });
            }

            // must return DTO
            return Ok(regionsDto);
        }

        // getting region by id
        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> getById([FromRoute] Guid Id)
        {
            /*
            var region = dbContext.Regions.Find(Id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
            */

            // getting data from database - domain model
            var regionsDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);


            if (regionsDomain == null)
            {
                return NotFound();
            }

            // converting model to DTO
            var regionsDto = new RegionsDto
            {
                Id = regionsDomain.Id,
                Code = regionsDomain.Code,
                Name = regionsDomain.Name,
                RegionImageUrl = regionsDomain.RegionImageUrl
            };

            


            // must return DTO
            return Ok(regionsDto);
        }



        // create new region resource
        [HttpPost]
        public async Task<IActionResult> create([FromBody] AddRregioRequestDto addRregioRequestDto)
        {
            // Map or convert DTO to domain model
            var regionDomainModel = new Region
            {
                Code = addRregioRequestDto.Code,
                Name = addRregioRequestDto.Name,
                RegionImageUrl = addRregioRequestDto.RegionImageUrl
            };


            // Use Domain model to create region

            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();


            // map domain model back to DTO
            var regionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(getById), new {id = regionDomainModel.Id}, regionDto);
        }

        // update an existing resource
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            // Check if region exists
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map DTO to domain model

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            // convert domain model to DTO
            var regionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        // delete a single region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> delete([FromRoute] Guid id)
        {
            var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //deleting
            dbContext.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            var regionDto = new RegionsDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

    }
}
