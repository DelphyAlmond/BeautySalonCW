using AutoMapper;
using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;

namespace BSUWebAppi.Adapters;

// Понадобится Маппер, чтобы переводить Binding model-и в Data model-и

public class CustomerAdapter : ICustomerAdapter
{
    private readonly ICustomerBLC _customerBusinessLogicContract;

    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    public CustomerAdapter(ICustomerBLC cBLC, ILogger<CustomerAdapter> logger, IMapper mapper)
    {
        _customerBusinessLogicContract = cBLC;
        _logger = logger;
        _mapper = mapper;
    }

    public CustomerOR GetList()
    {
        try
        {
            return CustomerOR.OK([.._customerBusinessLogicContract.GetAllCustomers().Select(
                x => _mapper.Map<CustomerVM>(x))]);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return CustomerOR.NotFound("< Ошибка - не инициализирован >"); // or bad request [ * ]
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CustomerOR.InternalServerError(
                $"< Ошибка при работе с SC (data storage): { ex.InnerException!.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CustomerOR.InternalServerError(ex.Message);
        }
    }

    public CustomerOR GetElement(string data)
    {
        try
        {
            return
            CustomerOR.OK(_mapper.Map<CustomerVM>(_customerBusinessLogicContract.GetCustomerByData(data)));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CustomerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CustomerOR.NotFound($"< Элемент за счёт: {data} - не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CustomerOR.InternalServerError(
                $"< Ошибка при работе с SC (data storage): {ex.InnerException!.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CustomerOR.InternalServerError(ex.Message);
        }
    }

    public CustomerOR RegisterCustomer(CustomerBM customer)
    {
        try
        {
            _customerBusinessLogicContract.InsertC(_mapper.Map<CustomerDM>(customer));
            return CustomerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CustomerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CustomerOR.BadRequest(
                $"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return CustomerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CustomerOR.BadRequest(
                $"< Ошибка при работе с SC (data storage): {ex.InnerException!.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CustomerOR.InternalServerError(ex.Message);
        }
    }

    public CustomerOR ChangeCustomerInfo(CustomerBM customer)
    {
        try
        {
            _customerBusinessLogicContract.UpdateC(_mapper.Map<CustomerDM>(customer));
            return CustomerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CustomerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CustomerOR.BadRequest(
                $"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CustomerOR.BadRequest($"< Элемент по ID:{customer.ID} - не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return CustomerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CustomerOR.BadRequest(
                $"< Ошибка при работе с SC (data storage): {ex.InnerException!.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CustomerOR.InternalServerError(ex.Message);
        }
    }

    public CustomerOR RemoveCustomer(string id)
    {
        try
        {
            _customerBusinessLogicContract.DeleteC(id);
            return CustomerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return CustomerOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return CustomerOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return CustomerOR.BadRequest($"< Элемент по ID:{id} - не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return CustomerOR.BadRequest($"< Ошибка при работе с SC (data storage): {ex.InnerException!.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return CustomerOR.InternalServerError(ex.Message);
        }
    }
}
