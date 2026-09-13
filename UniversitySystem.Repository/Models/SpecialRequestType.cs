namespace UniversitySystem.Repository.Models;

public partial class SpecialRequestType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<SpecialRequest> SpecialRequests { get; set; } = new List<SpecialRequest>();
}
