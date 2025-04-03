using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    // /api/walks
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

        // create walk
        // post method
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
