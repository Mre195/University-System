using System;
using System.Collections.Generic;

namespace UniversitySystem.Services.DTOs.Professor.Attendances
{
    public class AttendanceRecordDto
    {
        public Guid EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public List<AttendanceEntryDto> Record { get; set; } = new();
    }
}
