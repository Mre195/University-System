namespace UniversitySystem.Services.DTOs.Admin.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? HeadId { get; set; }
        public string? HeadName { get; set; }
    }
}
