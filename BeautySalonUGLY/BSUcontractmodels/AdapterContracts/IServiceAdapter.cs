using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IServiceAdapter
{
    // *also covers all BLC functionality, same>
    ServiceOR GetList();
    ServiceOR GetElement(string id);
    ServiceOR RegisterService(ServiceBM model);
    ServiceOR ChangeServiceInfo(ServiceBM model);
    ServiceOR RemoveService(string id);
}