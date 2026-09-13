using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Courses
{
    public class UpdateCourseDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, 6)]
        public int CreditHours { get; set; }
    }
}
