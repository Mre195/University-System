namespace UniversitySystem.Services.DTOs.Student.Sections
{
    public class AvailableSectionDto
    {
        public int SectionId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CreditHours { get; set; }
        public string Schedule { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public int RoomCapacity { get; set; }
        public int SeatsTaken { get; set; }
        public int SeatsAvailable { get; set; }
        public string ProfessorName { get; set; } = string.Empty;
    }
}
