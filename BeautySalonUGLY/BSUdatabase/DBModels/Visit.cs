using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Visit
{
    public required string ID { get; set; }
    public required string CustomerID { get; set; }
    public string? OrderID { get; set; }
    public required string WorkerID { get; set; } // [ * ] the master as well
    public required DateTime DateReg { get; set; }
    public required DateTime DatePlanned { get; set; }

    // services *

    [ForeignKey("VisitID")]
    public List<ServiceVisit>? Services { get; set; }

    public double Summ { get; set; }
    public double Discount { get; set; }
    public bool IsCanceled { get; set; }

    // --------------------------------------------- *

    public Worker? Worker { get; set; }
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
}
