using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Product
{
    public required string ID { get; set; }
    public required string ProductNaming { get; set; }
    public ProductType ProductType { get; set; }
    public required string ManufacturerID { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public bool IsDeleted { get; set; }

    // --------------------------------------------- *

    public Manufacturer? Manufacturer { get; set; } // < +

    [ForeignKey("ProductID")]
    public List<ProductOrder>? UnitsInOrder { get; set; }
}
