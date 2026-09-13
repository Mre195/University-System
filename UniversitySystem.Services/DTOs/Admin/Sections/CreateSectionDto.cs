using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Sections
{
    public class CreateSectionDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int ProfessorId { get; set; }

        [Required]
        public int SemesterId { get; set; }

        [Required]
        public string Schedule { get; set; } = string.Empty;

        [Required]
        public string RoomName { get; set; } = string.Empty;

        [Range(30, 60)]
        public int RoomCapacity { get; set; }
    }
}
