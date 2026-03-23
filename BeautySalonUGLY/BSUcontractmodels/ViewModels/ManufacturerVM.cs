namespace BSUcontractmodels.ViewModels;

public class ManufacturerVM
{
    // В отличии от Binding модели - сохр. поля версионности имени
    // для корректной внеш. передачи.
    public required string ID { get; set; }
    public required string Manufacturer { get; set; }
    public string? LastPrevNaming { get; set; }
    public string? SecondPrevNaming { get; set; }
}
