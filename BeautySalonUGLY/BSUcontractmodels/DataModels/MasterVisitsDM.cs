namespace BSUcontractmodels.DataModels;

public class MasterVisitsDM
{
    public required string MasterFullName { get; set; }
    public required List<string> Visits { get; set; }
}
