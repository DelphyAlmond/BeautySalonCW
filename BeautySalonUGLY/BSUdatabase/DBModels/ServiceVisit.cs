namespace BSUdatabase.DBModels;


internal class ServiceVisit
{
    public required string VisitID { get; set; }
    public required string ServiceID { get; set; }
    public required int Count { get; set; }

    // --------------------------------------------- *

    public Service? Service { get; set; }
    public Visit? Visit { get; set; }
}
