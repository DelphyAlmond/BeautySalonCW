using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Manufacturer
{
    public required string ID { get; set; }
    public required string CurrentName { get; set; }
    public string? LastPrevNaming { get; set; }
    public string? SecondPrevNaming { get; set; }

    // --------------------------------------------- *

    [ForeignKey("ManufacturerID")]
    public List<Product>? Products { get; set; }
}
