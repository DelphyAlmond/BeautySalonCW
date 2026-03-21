using BSUcontrmodels.Enums;

namespace BSUcontractmodels.ViewModels;

public class ProductVM
{
    public required string ID { get; set; }
    public required string ProductNaming { get; set; }
    public required ProductType ProductType { get; set; }
    public required string ManufacturerID { get; set; }
    public required double Price { get; set; }
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
}