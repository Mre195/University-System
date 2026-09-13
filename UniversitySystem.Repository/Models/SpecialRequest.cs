namespace UniversitySystem.Repository.Models;

public partial class SpecialRequest
{
    public Guid Id { get; set; }

    public int StudentId { get; set; }

    public int SpecialRequestTypeId { get; set; }

    public Guid? EnrollmentId { get; set; }

    public string Description { get; set; } = null!;

    public int SpecialRequestStatusId { get; set; }

    public DateTime SubmittedAt { get; set; }

    public virtual Enrollment? Enrollment { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual SpecialRequestStatus SpecialRequestStatus { get; set; } = null!;

    public virtual SpecialRequestType SpecialRequestType { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
