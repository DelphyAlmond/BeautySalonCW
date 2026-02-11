using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class ManufacturerDM(string id, string manufacturerNaming, string? lastPrevNaming,
    string? secondPrevNaming) : IValidation
{
    public string ID { get; private set; } = id;
    public string Manufacturer { get; private set; } = manufacturerNaming;
    public string LastPrevNaming { get; private set; } = lastPrevNaming;
    public string SecondPrevNaming { get; private set; } = secondPrevNaming;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Manufacturer: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Manufacturer: ID is not valid, not GUID >");
        if (Manufacturer.IsEmpty()) throw new ValidationException("< Manufacturer title is empty >");
    }
}
