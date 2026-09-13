namespace UniversitySystem.Services.DTOs.Admin.Courses
{
    public class CourseAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public int MajorId { get; set; }
        public string MajorName { get; set; } = string.Empty;
    }
}
