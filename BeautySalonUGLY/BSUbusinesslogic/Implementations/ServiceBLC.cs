using BSUcontractmodels.BusinessLogicContracts;
using BSUcontrmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ServiceBLC : IServiceBLC
{
    private readonly IServiceSC _serviceSC;

    public ServiceBLC(IServiceSC serviceSC)
    {
        _serviceSC = serviceSC;
    }

    public List<ServiceDM> getAllServices(bool onlyActive = true)
    {
        return _serviceSC.GetList(onlyActive);
    }

    public ServiceDM GetServiceByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Service BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _serviceSC.GetElementByID(data);
            if (byId != null) return byId;
        }

        var byName = _serviceSC.GetElementByName(data);
        if (byName != null) return byName;

        throw new ValidationException($"< Service with data '{data}' not found >");
    }

    public void InsertS(ServiceDM service)
    {
        service.Validate();
        _serviceSC.AddElement(service);
    }

    public void UpdateS(ServiceDM service)
    {
        service.Validate();
        _serviceSC.UpdElement(service);
    }

    public void DeleteS(string id)
    {
        if (id.IsEmpty())
            throw new ValidationException("< Service BLC: ID for deletion is empty >");
        _serviceSC.DelElement(id);
    }
}