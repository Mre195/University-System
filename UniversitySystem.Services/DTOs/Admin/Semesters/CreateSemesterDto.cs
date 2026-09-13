using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Admin.Semesters
{
    public class CreateSemesterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; } = false;
    }
}
