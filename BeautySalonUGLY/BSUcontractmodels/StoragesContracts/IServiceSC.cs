using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IServiceSC
{
    List<ServiceDM> GetList(bool onlyActive = true);
    ServiceDM? GetElementByID(string id);
    ServiceDM? GetElementByName(string name);

    void AddElement(ServiceDM service);
    void UpdElement(ServiceDM service);
    void DelElement(string id);
}