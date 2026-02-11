using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IServiceBLC
{
    List<ServiceDM> getAllServices(bool onlyActive = true);
    ServiceDM GetServiceByData(string data);
    void InsertS(ServiceDM product);
    void UpdateS(ServiceDM product);
    void DeleteS(string id);
}
