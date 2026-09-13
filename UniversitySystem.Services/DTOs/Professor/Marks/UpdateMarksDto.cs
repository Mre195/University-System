using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Professor.Marks
{
    public class UpdateMarksDto
    {
        [Required]
        public Guid EnrollmentId { get; set; }

        [Range(0, 100)]
        public int AssignmentMark { get; set; }

        [Range(0, 100)]
        public int MidExamMark { get; set; }

        [Range(0, 100)]
        public int FinalExamMark { get; set; }
    }
}
