using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class ServUnitVisitLinkDM(string visitId, string serviceId, int count, double price) : IValidation
{
    private readonly ServiceDM? _service;

    // Вспомогательная сущность, без своего уник. идентификатора (no ID needed)
    public string VisitID { get; private set; } = visitId;
    public string ServiceID { get; private set; } = serviceId;
    public int Count { get; private set; } = count;

    public double Price { get; private set; } = price;

    public string? ServiceName => _service?.ServiceNaming;

    public ServUnitVisitLinkDM(string orderId, string productId, int count, double price, ServiceDM service)
        : this(orderId, productId, count, price)
    {
        _service = service;
    }

    public void Validate()
    {
        if (VisitID.IsEmpty()) throw new ValidationException("< V-Link: Visit ID is empty >");
        if (!VisitID.IsGuid()) throw new ValidationException("< V-Link: Visit ID is not valid >");
        if (ServiceID.IsEmpty()) throw new ValidationException("< V-Link: Service ID is empty >");
        if (!ServiceID.IsGuid()) throw new ValidationException("< V-Link: Service ID is not valid >");
        if (Count <= 0) throw new ValidationException("< V-Link: Count is negative or equal to 0 - no service unit >");
    }
}
