namespace UniversitySystem.Repository.Models;

public partial class Enrollment
{
    public Guid Id { get; set; }

    public int StudentId { get; set; }

    public int SectionId { get; set; }

    public int AssignmentMark { get; set; }

    public int MidExamMark { get; set; }

    public int FinalExamMark { get; set; }

    public string Grade { get; set; } = null!;

    public int EnrollmentStatusId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual EnrollmentStatus EnrollmentStatus { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<SpecialRequest> SpecialRequests { get; set; } = new List<SpecialRequest>();

    public virtual Student Student { get; set; } = null!;
}
