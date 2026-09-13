using System;

namespace UniversitySystem.Services.DTOs.Shared.Sections
{
    public class SectionSummaryDto
    {
        public int SectionId { get; set; }
        public string? CourseName { get; set; }
        public int CreditHours { get; set; }
        public string? Schedule { get; set; }
        public string? RoomName { get; set; }
        public string? ProfessorName { get; set; }
        public string? SemesterName { get; set; }
        public int EnrolledCount { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
