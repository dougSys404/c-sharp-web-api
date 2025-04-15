using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Data;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;
using NZWalks.Api.Repositories;
using System.Text.Json;

namespace NZWalks.Api.Controllers
{
    // https://localhost:7121/api/regions
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepository regionRepository, 
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // getting all regions using list
        [HttpGet]
        //[Authorize (Roles = "Reader, Writer")]
        public async Task<IActionResult> getAll()
        {

            logger.LogInformation("GetAll Regions Action Method Was Invoked");

            // getting data from database - domain model
            var regionsDomain = await regionRepository.GetAllAsync();

            logger.LogInformation($"Finished GetAll Regions Request With Data: {JsonSerializer.Serialize(regionsDomain)}");

            // return DTOs
            return Ok(mapper.Map<List<RegionsDto>>(regionsDomain));
        }


        // getting region by id
        [HttpGet]
        [Route("{Id}")]
        //[Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> getById([FromRoute] Guid Id)
        {
           
            var regionsDomain = await regionRepository.GetByIdAsync(Id);


            if (regionsDomain == null)
            {
                return NotFound();
            }

            // return DTO to client
            return Ok(mapper.Map<RegionsDto>(regionsDomain));
        }



        // create new region resource
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRregioRequestDto addRregioRequestDto)
        {
            

            // Map or convert DTO to domain model
            var regionDomainModel = mapper.Map<Region>(addRregioRequestDto);

            // Use Domain model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // map domain model back to DTO
            var regionDto = mapper.Map<RegionsDto>(regionDomainModel);

            return CreatedAtAction(nameof(getById), new {id = regionDomainModel.Id}, regionDto);
            
            
        }

        // update an existing resource
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto) 
        {
            
                // Mapping dto do domain model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                // Check if region exists
                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                // Map DTO to domain model

                regionDomainModel.Code = updateRegionRequestDto.Code;
                regionDomainModel.Name = updateRegionRequestDto.Name;
                regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

                await dbContext.SaveChangesAsync();

                // convert and return domain model to DTO
                return Ok(mapper.Map<RegionsDto>(regionDomainModel));
           
        }

        // delete a single region
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> delete([FromRoute] Guid id)
        {

            // Check if region exists
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionsDto>(regionDomainModel);

            return Ok(regionDto);
        }

    }
}
