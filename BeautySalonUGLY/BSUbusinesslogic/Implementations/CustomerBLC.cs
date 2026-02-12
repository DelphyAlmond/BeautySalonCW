using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System.Text.RegularExpressions;

namespace BSUbusinesslogic.Implementations;

public class CustomerBLC(ICustomerSC customerSC) : ICustomerBLC
{
    private readonly ICustomerSC _customerSC = customerSC;

    public List<CustomerDM> GetAllCustomers()
    {
        return _customerSC.GetCustomers() ?? throw new NullListException();
    }

    public CustomerDM GetCustomerByData(string data)
    {
        if (data.IsEmpty())
            throw new ArgumentNullException($"< Customer BLC: search data - {nameof(data)}, is empty >");

        // Проверяем, является ли строка GUID
        if (data.IsGuid())
        {
            return _customerSC.GetCByID(data) ?? throw new ElementNotFoundException(null, data);
        }

        // / похоже ли на номер телефона
        if (Regex.IsMatch(data, @"^(?:\+7|8)[\s\-]*(?:\(\d{3}\)|\d{3})[\s\-]*\d{3}[\s\-]*\d{2}[\s\-]*\d{2}$"))
        {
            return _customerSC.GetCByPhone(data) ?? throw new ElementNotFoundException(null, data);
        }

        // Иначе ищем по имени
        return _customerSC.GetCByName(data) ?? throw new ElementNotFoundException($"< Customer with data '{data}' not found >", data);
    }

    public void InsertC(CustomerDM customer)
    {
        customer.Validate();
        _customerSC.AddC(customer);
    }

    public void UpdateC(CustomerDM customer)
    {
        customer.Validate();
        _customerSC.UpdC(customer);
    }

    public void DeleteC(string id)
    {
        if (id.IsEmpty() || !id.IsGuid())
            throw new ValidationException("< Customer BLC: ID for deletion is empty or not valid >");
        _customerSC.DelC(id);
    }
}
