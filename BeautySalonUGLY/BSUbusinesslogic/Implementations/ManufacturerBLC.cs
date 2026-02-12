using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ManufacturerBLC(IManufacturerSC manufacturerSC) : IManufacturerBLC
{
    private readonly IManufacturerSC _manufacturerSC = manufacturerSC;

    public List<ManufacturerDM> GetAllManufacturers()
    {
        return _manufacturerSC.GetManufacturers() ?? throw new NullListException();
    }

    public ManufacturerDM GetManufacturersByData(string data)
    {
        if (data.IsEmpty())
            throw new ArgumentNullException($"< Manufacturer BLC: search data - {nameof(data)}, is empty >");

        if (data.IsGuid())
        {
            return _manufacturerSC.GetMByID(data) ?? throw new ElementNotFoundException(null, data);
        }
        var byName = _manufacturerSC.GetMByName(data);
        if (byName != null) return byName;
        byName = _manufacturerSC.GetMPrevName(data);
        return byName ?? throw new ElementNotFoundException($"< Manufacturer with data '{data}' not found >", data);
    }

    public void InsertM(ManufacturerDM manufacturer)
    {
        manufacturer.Validate();
        _manufacturerSC.AddM(manufacturer);
    }

    public void UpdateM(ManufacturerDM manufacturer)
    {
        manufacturer.Validate();
        _manufacturerSC.UpdM(manufacturer);
    }

    public void DeleteM(string id)
    {
        if (id.IsEmpty() || !id.IsGuid())
            throw new ValidationException("< Manufacturer BLC: ID for deletion is empty or not valid >");
        _manufacturerSC.DelM(id);
    }
}