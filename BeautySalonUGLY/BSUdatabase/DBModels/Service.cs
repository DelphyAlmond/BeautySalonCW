using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Service
{
    public required string ID { get; set; }
    public required string ServiceNaming { get; set;}
    public double Price { get; set; }
    public string Description { get; set; }
    public required int Duration { get; set; }
    public bool IsDeleted { get; set; }

    // --------------------------------------------- *

    [ForeignKey("ServiceID")]
    public List<ServiceVisit>? UnitsInVisit { get; set; }
}
