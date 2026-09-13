namespace UniversitySystem.Repository.Models;

public partial class Student
{
    public int Id { get; set; }

    public bool IsActive { get; set; }

    public int MajorId { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Major Major { get; set; } = null!;

    public virtual ICollection<SpecialRequest> SpecialRequests { get; set; } = new List<SpecialRequest>();

    public virtual User User { get; set; } = null!;
}
