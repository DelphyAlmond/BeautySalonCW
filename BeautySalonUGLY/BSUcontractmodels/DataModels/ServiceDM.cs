using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System.Text.RegularExpressions;

namespace BSUcontrmodels.DataModels;

public class ServiceDM(string id, string serviceNaming, ProductType prType, double price, string description,
    int minutes, bool isDeleted) : IValidation
{
    public string ID { get; private set; } = id;
    public string ServiceNaming { get; private set; } = serviceNaming;
    public ProductType ProductType { get; private set; } = prType;
    public double Price { get; private set; } = price;
    public string Description { get; private set; } = description;
    public int Duration { get; private set; } = minutes;
    public bool IsDeleted { get; private set; } = isDeleted;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Service: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Service: ID is not valid, not GUID >");
        if (ServiceNaming.IsEmpty()) throw new ValidationException("< Naming of the service is empty >");
        if (ProductType == ProductType.None) throw new ValidationException("< Type of the service is not stated >");
        if (Price <= 0) throw new ValidationException("< Service: Price is negative or not stated >");
        if (Duration <= 0) throw new ValidationException("< Duration(minutes) is negative or equal to 0 >");
    }
}
