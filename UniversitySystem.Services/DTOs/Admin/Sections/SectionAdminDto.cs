namespace UniversitySystem.Services.DTOs.Admin.Sections
{
    public class SectionAdminDto
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public string SemesterName { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int RoomCapacity { get; set; }
        public int EnrolledCount { get; set; }
    }
}
