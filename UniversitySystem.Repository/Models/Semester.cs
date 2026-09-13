namespace UniversitySystem.Repository.Models;

public partial class Semester
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
