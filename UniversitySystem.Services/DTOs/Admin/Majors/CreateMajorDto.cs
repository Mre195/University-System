using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Majors
{
    public class CreateMajorDto
    {
        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(120, 263)]
        public int Hours { get; set; }
    }
}
