using NZWalks.Api.Model.Domain;

namespace NZWalks.Api.Model.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthKm { get; set; }
        public string? WalkImageUrl { get; set; }



        //Navigation properties
        public RegionsDto Region { get; set; }

        public DifficultyDto Difficulty { get; set; }

    }
}
