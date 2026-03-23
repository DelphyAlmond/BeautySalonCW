namespace BSUcontractmodels.ViewModels;

public class ProdUnitOrderLinkVM
{
    // [ ! ] Вложенные VM содержат не только ID,
    // но и имена/цены –> типичная денормализация для API.
    public required string ProductID { get; set; }
    public required string ProductName { get; set; }
    public required int Count { get; set; }
    public required double Price { get; set; }
    public double Total => Price * Count;
}