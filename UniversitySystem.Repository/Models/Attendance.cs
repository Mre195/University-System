namespace UniversitySystem.Repository.Models;

public partial class Attendance
{
    public Guid Id { get; set; }

    public Guid EnrollmentId { get; set; }

    public DateTime Date { get; set; }

    public bool IsPresent { get; set; }

    public virtual Enrollment Enrollment { get; set; } = null!;
}
