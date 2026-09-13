using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Majors
{
    public class UpdateMajorDto
    {
        public string? Name { get; set; }

        [Range(120, 263)]
        public int? Hours { get; set; }
    }
}
