using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class ServUnitVisitLinkDM(string visitId, string serviceId, int count) : IValidation
{
    // Вспомогательная сущность, без своего уник. идентификатора (no ID needed)
    public string VisitID { get; private set; } = visitId;
    public string ServiceID { get; private set; } = serviceId;
    public int Count { get; private set; } = count;

    public void Validate()
    {
        if (VisitID.IsEmpty()) throw new ValidationException("< V-Link: Visit ID is empty >");
        if (!VisitID.IsGuid()) throw new ValidationException("< V-Link: Visit ID is not valid >");
        if (ServiceID.IsEmpty()) throw new ValidationException("< V-Link: Service ID is empty >");
        if (!ServiceID.IsGuid()) throw new ValidationException("< V-Link: Service ID is not valid >");
        if (Count <= 0) throw new ValidationException("< V-Link: Count is negative or equal to 0 - no service unit >");
    }
}
