
namespace Primitive.Entities;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public string? Image { get; set; }
    public long ProductGroupId { get; set; }
    public long BrandId { get; set; }
}
