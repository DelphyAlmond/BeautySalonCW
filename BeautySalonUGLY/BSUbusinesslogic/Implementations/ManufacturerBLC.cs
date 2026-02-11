using BSUcontractmodels.BusinessLogicContracts;
using BSUcontrmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ManufacturerBLC : IManufacturerBLC
{
    private readonly IManufacturerSC _manufacturerSC;

    public ManufacturerBLC(IManufacturerSC manufacturerSC)
    {
        _manufacturerSC = manufacturerSC;
    }

    public List<ManufacturerDM> GetAllManufacturers()
    {
        return _manufacturerSC.GetManufacturers();
    }

    public ManufacturerDM GetManufacturersByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Manufacturer BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _manufacturerSC.GetMByID(data);
            if (byId != null) return byId;
        }

        var byName = _manufacturerSC.GetMByName(data);
        if (byName != null) return byName;

        var byPrevName = _manufacturerSC.GetMPrevName(data);
        if (byPrevName != null) return byPrevName;

        throw new ValidationException($"< Manufacturer with data '{data}' not found >");
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
        if (id.IsEmpty())
            throw new ValidationException("< Manufacturer BLC: ID for deletion is empty >");
        _manufacturerSC.DelM(id);
    }
}