using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface ICustomerSC
{
    List<CustomerDM> GetCustomers();
    CustomerDM? GetCByID(string id);
    CustomerDM? GetCByName(string username);
    CustomerDM? GetCByPhone(string phoneNumber);

    void AddC(CustomerDM customer);
    void UpdC(CustomerDM customer);
    void DelC(string id);
}
