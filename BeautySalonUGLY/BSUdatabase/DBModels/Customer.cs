using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Customer
{
    public required string ID { get; set; }
    public required string Username { get; set; }
    public required string Phonenumber { get; set; }
    public double Bonuses { get; set; }

    // --------------------------------------------- *

    [ForeignKey("CustomerID")]
    public List<Order>? Orders { get; set; }

    [ForeignKey("CustomerID")]
    public List<Visit>? Visits { get; set; }
}
