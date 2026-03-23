using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class ProdUnitOrderLinkDM(string orderId, string productId, int count, double price) : IValidation
{
    private readonly ProductDM? _product;

    // Вспомогательная сущность, без своего уник. идентификатора (no ID needed)
    public string OrderID { get; private set; } = orderId;
    public string ProductID { get; private set; } = productId;
    public int Count { get; private set; } = count;

    public double Price { get; private set; } = price;

    public string? ProductName => _product?.ProductNaming;

    public ProdUnitOrderLinkDM(string orderId, string productId, int count, double price, ProductDM product)
        : this(orderId, productId, count, price)
    {
        _product = product;
    }

    public void Validate()
    {
        if (OrderID.IsEmpty()) throw new ValidationException("< O-Link: Order ID is empty >");
        if (!OrderID.IsGuid()) throw new ValidationException("< O-Link: Order ID is not valid >");
        if (ProductID.IsEmpty()) throw new ValidationException("< O-Link: Product ID is empty >");
        if (!ProductID.IsGuid()) throw new ValidationException("< O-Link: Product ID is not valid >");
        if (Count <= 0) throw new ValidationException("< O-Link: Count is negative or equal to 0 - no product unit >");
    }
}
