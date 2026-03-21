namespace BSUcontractmodels.ViewModels;

public class VisitVM
{
    public required string ID { get; set; }
    public required string CustomerID { get; set; }
    public string? CustomerName { get; set; }
    public string? OrderID { get; set; }
    public string? WorkerID { get; set; }
    public string? WorkerName { get; set; }
    public required DateTime DateReg { get; set; }
    public required DateTime DatePlanned { get; set; }
    public required List<ServUnitVisitLinkVM> Services { get; set; }
    public required double Summ { get; set; }
    public required double Discount { get; set; }
    public required bool IsCanceled { get; set; }
}