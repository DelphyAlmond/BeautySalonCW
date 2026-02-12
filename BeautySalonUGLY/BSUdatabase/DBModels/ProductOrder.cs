namespace BSUdatabase.DBModels;


internal class ProductOrder
{
    public required string OrderID { get; set; }
    public required string ProductID { get; set; }
    public required int Count { get; set; }

    // --------------------------------------------- *

    public Product? Product { get; set; }
    public Order? Order { get; set; }
}
