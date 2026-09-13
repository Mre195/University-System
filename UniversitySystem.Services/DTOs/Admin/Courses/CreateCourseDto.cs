using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Courses
{
    public class CreateCourseDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 6)]
        public int CreditHours { get; set; }

        [Required]
        public int MajorId { get; set; }
    }
}
