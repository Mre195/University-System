using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Sections
{
    public class UpdateSectionDto
    {
        public int? ProfessorId { get; set; }
        public string? Schedule { get; set; }
        public string? RoomName { get; set; }

        [Range(30, 60)]
        public int? RoomCapacity { get; set; }
    }
}
