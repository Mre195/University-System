using System;

namespace UniversitySystem.Services.DTOs.Admin.Students
{
    public class StudentAdminDto
    {
        public int StudentId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
        public int MajorId { get; set; }
        public bool IsActive { get; set; }
    }
}
