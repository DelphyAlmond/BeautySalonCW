using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace BSUdatabase.DBModels;

internal class Order
{
    public required string ID { get; set; }
    public string? CustomerID { get; set; }
    public required string WorkerID { get; set; }
    public string? MasterID { get; set; }
    public DateTime Date { get; set; }

    // prod-units in the cart *

    [ForeignKey("OrderID")]
    public List<ProductOrder>? Cart { get; set; }

    public double Summ { get; set; }
    public double Discount { get; set; }
    public OrderStatus Status { get; set; }

    // --------------------------------------------- *

    public Worker? Worker { get; set; }
    public Worker? Master { get; set; } // *
    public Customer? Customer { get; set; }
}
