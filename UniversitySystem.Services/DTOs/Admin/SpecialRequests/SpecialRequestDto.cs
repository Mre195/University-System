using System;

namespace UniversitySystem.Services.DTOs.Admin.SpecialRequests
{
    public class SpecialRequestDto
    {
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
    }
}
