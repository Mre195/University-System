namespace UniversitySystem.Repository.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CreditHours { get; set; }

    public int MajorId { get; set; }

    public virtual Major Major { get; set; } = null!;

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
