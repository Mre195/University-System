namespace UniversitySystem.Repository.Models;

public partial class Department
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? HeadId { get; set; }

    public virtual Professor? Head { get; set; }

    public virtual ICollection<Major> Majors { get; set; } = new List<Major>();

    public virtual ICollection<Professor> Professors { get; set; } = new List<Professor>();
}
