namespace UniversitySystem.Services.DTOs.Student.Courses
{
    public class CompletedCourseDto
    {
        public int StudentId { get; set; }
        public string? CourseName { get; set; }
        public int CreditHours { get; set; }
        public int TotalMarks { get; set; }
    }
}
