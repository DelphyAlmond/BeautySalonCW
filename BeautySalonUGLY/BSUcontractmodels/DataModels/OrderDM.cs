using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class OrderDM(string id, string? customerID, string workerID, string? staffID, List<ProdUnitOrderLinkDM> list,
    double summ, double bonuses, OrderStatus status) : IValidation
{
    public string ID { get; private set; } = id;
    public string? CustomerID { get; private set; } = customerID;
    public string WorkerID { get; private set; } = workerID;
    public string? MasterID { get; private set; } = staffID;
    public DateTime Date { get; private set; } = DateTime.UtcNow;

    // Поскольку пользователи имеют возможность возыметь чек на покупаемую косметику,
    // список (корзина cart) продуктов в выдаче (order) и будет представлять из себя чек
    public List<ProdUnitOrderLinkDM> Cart { get; private set; } = list;
    // Предусмотрена также и выдача материалов мастерам (masterID) услуги, в случае надобности

    public double Summ { get; private set; } = summ;
    public double Discount { get; private set; } = bonuses;
    public OrderStatus Status { get; private set; } = status;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Order: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Order: ID is not valid, not GUID >");
        if (!CustomerID?.IsGuid() ?? !CustomerID?.IsEmpty() ?? false)
            throw new ValidationException("< Order: Customer ID is not valid >");
        if (WorkerID.IsEmpty()) throw new ValidationException("< Order: Worker ID is empty >");
        if (!WorkerID.IsGuid()) throw new ValidationException("< Order: Worker ID is not valid, not GUID >");
        if (!MasterID?.IsGuid() ?? !MasterID?.IsEmpty() ?? false)
            throw new ValidationException("< Order: Master ID is not valid >");

        if ((Cart?.Count ?? 0) == 0) throw new ValidationException("< Order: Cart is empty, no products chosen >");

        if (Summ <= 0) throw new ValidationException("< Order: Summ less or equal to 0, no products chosen >");
        // < ? >
        if (Status == OrderStatus.Canceled) throw new ValidationException("< Status is not valid, can't be Canceled at first >");
    }
}
