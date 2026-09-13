using System;

namespace UniversitySystem.Services.DTOs.Student.Enrollments
{
    public class EnrollmentDto
    {
        public Guid EnrollmentId { get; set; }
        public int SectionId { get; set; }
        public int StudentId { get; set; }
        public int ProfessorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
