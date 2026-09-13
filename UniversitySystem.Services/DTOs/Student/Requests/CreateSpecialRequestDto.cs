using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Student.Requests
{
    public class CreateSpecialRequestDto
    {
        [Required]
        public int SpecialRequestTypeId { get; set; }

        public Guid? EnrollmentId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
