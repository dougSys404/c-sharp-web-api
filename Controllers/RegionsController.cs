﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Data;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    // https://localhost:7121/api/regions
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // getting all regions using list
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            // getting data from database - domain model
            var regionsDomain = await regionRepository.GetAllAsync();

            // return DTOs
            return Ok(mapper.Map<List<RegionsDto>>(regionsDomain));
        }


        // getting region by id
        [HttpGet]
        [Route("{Id}")]
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
