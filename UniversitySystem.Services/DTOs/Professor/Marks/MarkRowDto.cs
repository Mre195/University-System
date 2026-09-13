using System;

namespace UniversitySystem.Services.DTOs.Professor.Marks
{
    public class MarkRowDto
    {
        public Guid EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int AssignmentMark { get; set; }
        public int MidExamMark { get; set; }
        public int FinalExamMark { get; set; }
        public string LetterGrade { get; set; } = string.Empty;
    }
}
