namespace UniversitySystem.Repository.Models;

public partial class Professor
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public Guid UserId { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    public virtual User User { get; set; } = null!;
}
