using System.ComponentModel.DataAnnotations;

namespace NZWalks.Api.Model.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Name has to be a maximum of 1000 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public double LengthKm { get; set; }
        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
