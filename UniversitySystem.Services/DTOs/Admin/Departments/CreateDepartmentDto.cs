using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Departments
{
    public class CreateDepartmentDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public int? HeadId { get; set; }
    }
}
