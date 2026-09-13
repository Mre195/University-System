namespace UniversitySystem.Repository.Models;

public partial class Section
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public int ProfessorId { get; set; }

    public int SemesterId { get; set; }

    public string Schedule { get; set; } = null!;

    public string RoomName { get; set; } = null!;

    public int RoomCapacity { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Professor Professor { get; set; } = null!;

    public virtual Semester Semester { get; set; } = null!;
}
