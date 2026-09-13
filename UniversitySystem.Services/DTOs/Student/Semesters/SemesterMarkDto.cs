namespace UniversitySystem.Services.DTOs.Student.Semesters
{
    public class SemesterMarkDto
    {
        public int SectionId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int AssignmentMark { get; set; }
        public int MidExamMark { get; set; }
        public int FinalExamMark { get; set; }
        public string Grade { get; set; } = string.Empty;
        public int Absences { get; set; }
    }
}
