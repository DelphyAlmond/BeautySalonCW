namespace BSUcontractmodels.ViewModels;

public class ServiceVM
{
    public required string ID { get; set; }
    public required string ServiceNaming { get; set; }
    public required double Price { get; set; }
    public string? Description { get; set; }
    public required int Duration { get; set; }
    public required bool IsDeleted { get; set; }
}