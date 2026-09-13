using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Professor.Attendances
{
    public class UpdateAttendanceDto
    {
        [Required]
        public Guid EnrollmentId { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        public DateTime? Date { get; set; }
    }
}
