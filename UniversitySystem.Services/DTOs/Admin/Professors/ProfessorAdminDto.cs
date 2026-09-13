namespace UniversitySystem.Services.DTOs.Admin.Professors
{
    public class ProfessorAdminDto
    {
        public int ProfessorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
