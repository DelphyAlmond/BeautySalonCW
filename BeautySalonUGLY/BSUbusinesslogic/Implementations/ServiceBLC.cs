using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class ServiceBLC(IServiceSC serviceSC) : IServiceBLC
{
    private readonly IServiceSC _serviceSC = serviceSC;

    public List<ServiceDM> GetAllServices(bool onlyActive = true)
    {
        return _serviceSC.GetServices(onlyActive) ?? throw new NullListException();
    }

    public ServiceDM GetServiceByData(string data)
    {
        if (data.IsEmpty())
            throw new ArgumentNullException($"< Service BLC: search data - {nameof(data)}, is empty >");

        if (data.IsGuid())
        {
            return _serviceSC.GetSByID(data) ?? throw new ElementNotFoundException(null, data);
        }
        return _serviceSC.GetSByName(data) ?? throw new ElementNotFoundException($"< Service with data '{data}' not found >", data);
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
        if (id.IsEmpty() || !id.IsGuid())
            throw new ValidationException("< Service BLC: ID for deletion is empty or not valid >");
        _serviceSC.DelElement(id);
    }
}