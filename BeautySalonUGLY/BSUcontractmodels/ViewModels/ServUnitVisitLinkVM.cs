namespace BSUcontractmodels.ViewModels;

public class ServUnitVisitLinkVM
{
    public required string ServiceID { get; set; }
    public required string ServiceName { get; set; }
    public required double Price { get; set; }
    public required int Count { get; set; }
    public double Total => Price * Count;
}