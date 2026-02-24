using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IServiceSC
{
    List<ServiceDM> GetServices(bool onlyActive = true);
    ServiceDM? GetSByID(string id);
    ServiceDM? GetSByName(string name);

    void AddElement(ServiceDM service);
    void UpdElement(ServiceDM service);
    void DelElement(string id);
}