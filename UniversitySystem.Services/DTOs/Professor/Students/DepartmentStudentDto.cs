namespace UniversitySystem.Services.DTOs.Professor.Students
{
    public class DepartmentStudentDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MajorName { get; set; } = string.Empty;
    }
}
