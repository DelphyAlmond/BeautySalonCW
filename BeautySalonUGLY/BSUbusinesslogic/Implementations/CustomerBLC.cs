using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System.Text.RegularExpressions;

namespace BSUbusinesslogic.Implementations;

public class CustomerBLC : ICustomerBLC
{
    private readonly ICustomerSC _customerSC;

    public CustomerBLC(ICustomerSC customerSC)
    {
        _customerSC = customerSC;
    }

    public List<CustomerDM> GetAllCustomers()
    {
        return _customerSC.GetCustomers();
    }

    public CustomerDM GetCustomerByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Customer BLC: search data is empty >");

        // Проверяем, является ли строка GUID
        if (data.IsGuid())
        {
            var byId = _customerSC.GetCByID(data);
            if (byId != null) return byId;
        }

        // / похоже ли на номер телефона
        if (Regex.IsMatch(data, @"^(?:\+7|8)[\s\-]*(?:\(\d{3}\)|\d{3})[\s\-]*\d{3}[\s\-]*\d{2}[\s\-]*\d{2}$"))
        {
            var byPhone = _customerSC.GetCByPhone(data);
            if (byPhone != null) return byPhone;
        }

        // Иначе ищем по имени
        var byName = _customerSC.GetCByName(data);
        if (byName != null) return byName;

        throw new ValidationException($"< Customer with data '{data}' not found >");
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
        if (id.IsEmpty())
            throw new ValidationException("< Customer BLC: ID for deletion is empty >");
        _customerSC.DelC(id);
    }
}
