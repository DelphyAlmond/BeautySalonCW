using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface ICustomerBLC
{
    List<CustomerDM> GetAllCustomers();

    // неизв. заранее данные *(id / name / phone number ...)
    CustomerDM GetCustomerByData(string data);
    void InsertC(CustomerDM customer);
    void UpdateC(CustomerDM customer);
    void DeleteC(string id);
}
