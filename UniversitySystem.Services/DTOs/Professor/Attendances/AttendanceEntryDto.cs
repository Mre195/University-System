using System;

namespace UniversitySystem.Services.DTOs.Professor.Attendances
{
    public class AttendanceEntryDto
    {
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
