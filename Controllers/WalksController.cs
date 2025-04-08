using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IMapper mapper, IWalkRepository walkRepository) 
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }



        // get all walks
        // get method
        // /api/walks?filterOn=Name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {

            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery);

            // Map domain to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // get walk by Id
        //get method
        // /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // mapping model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // create walk
        // post method
        // /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            
                // Map DTO to Domain Model
                var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

                await walkRepository.CreateAsync(walkDomainModel);

                // map domain model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
             

        }

        // updating and existing resource by id
        // Put method
        [HttpPut]
        [Route("{Id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid Id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

                await walkRepository.UpdateAsync(Id, walkDomainModel);

                if (walkDomainModel == null)
                {
                    return NotFound();
                }

                // map domain model to DTO
                return Ok(mapper.Map<WalkDto>(walkDomainModel));
            
        }

        // deleting and existing resource by id
        // Delete method
        [HttpDelete]
        [Route("{Id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var deleteWalkDomainModel =  await walkRepository.DeleteAsync(Id);

            if (deleteWalkDomainModel == null)
            {
                return NotFound();
            }

            // map
            return Ok(mapper.Map<WalkDto>(deleteWalkDomainModel));
        }

    }
}
