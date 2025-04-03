using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // /api/walks
        [HttpGet]
        public async Task<IActionResult> getAll()
        {

            var walksDomainModel = await walkRepository.getAllAsync();

            // Map domain to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomainModel));
        }

        // get walk by Id
        //get method
        // /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> getById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.getByIdAsync(id);

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
        public async Task<IActionResult> create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            // Map DTO to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            // map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }
    }
}
