namespace BSUcontractmodels.ViewModels;

public class ManufacturerVM
{
    public required string ID { get; set; }
    public required string Manufacturer { get; set; }
    public string? LastPrevNaming { get; set; }
    public string? SecondPrevNaming { get; set; }
}
