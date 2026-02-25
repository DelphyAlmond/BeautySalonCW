using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IManufacturerSC
{
    List<ManufacturerDM> GetManufacturers();
    ManufacturerDM? GetMByID(string id);
    ManufacturerDM? GetMByName(string naming);
    ManufacturerDM? GetMByPrevName(string naming);

    void AddM(ManufacturerDM manufacturer);
    void UpdM(ManufacturerDM manufacturer);
    void DelM(string id);
}

