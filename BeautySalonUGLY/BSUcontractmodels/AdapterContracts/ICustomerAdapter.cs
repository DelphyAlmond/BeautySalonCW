using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface ICustomerAdapter
{
    // *BLC reference: GetAllCustomers(), GetCustomerByData(string data),
    // InsertC(CustomerDM customer), UpdateC(CustomerDM customer), DeleteC(string id)
    CustomerOR GetList();
    CustomerOR GetElement(string data);
    CustomerOR RegisterCustomer(CustomerBM buyerModel);
    CustomerOR ChangeCustomerInfo(CustomerBM buyerModel);
    CustomerOR RemoveCustomer(string id);
}
