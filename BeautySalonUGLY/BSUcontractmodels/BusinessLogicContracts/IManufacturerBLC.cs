using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IManufacturerBLC
{
    List<ManufacturerDM> GetAllManufacturers();
    ManufacturerDM GetManufacturersByData(string data);
    void InsertM(ManufacturerDM manufacturer);
    void UpdateM(ManufacturerDM manufacturer);
    void DeleteM(string id);
}
