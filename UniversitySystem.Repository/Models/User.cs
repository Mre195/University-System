namespace UniversitySystem.Repository.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Admin? Admin { get; set; }

    public virtual Professor? Professor { get; set; }

    public virtual Student? Student { get; set; }
}
