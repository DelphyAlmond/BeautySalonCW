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

public class ServiceAdapter : IServiceAdapter
{
    private readonly IServiceBLC _serviceBLC;
    private readonly ILogger<ServiceAdapter> _logger;
    private readonly IMapper _mapper;

    public ServiceAdapter(IServiceBLC serviceBLC, ILogger<ServiceAdapter> logger, IMapper mapper)
    {
        _serviceBLC = serviceBLC;
        _logger = logger;
        _mapper = mapper;
    }

    public ServiceOR GetList()
    {
        try
        {
            var services = _serviceBLC.GetAllServices(false);
            var vms = services.Select(x => _mapper.Map<ServiceVM>(x)).ToList();
            return ServiceOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ServiceOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ServiceOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ServiceOR.InternalServerError(ex.Message);
        }
    }

    public ServiceOR GetElement(string id)
    {
        try
        {
            var service = _serviceBLC.GetServiceByData(id);
            return ServiceOR.OK(_mapper.Map<ServiceVM>(service));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ServiceOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ServiceOR.NotFound($"< Элемент по ID: {id} - не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ServiceOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ServiceOR.InternalServerError(ex.Message);
        }
    }

    public ServiceOR RegisterService(ServiceBM model)
    {
        try
        {
            var dm = _mapper.Map<ServiceDM>(model);
            _serviceBLC.InsertS(dm);
            return ServiceOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ServiceOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ServiceOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ServiceOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ServiceOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ServiceOR.InternalServerError(ex.Message);
        }
    }

    public ServiceOR ChangeServiceInfo(ServiceBM model)
    {
        try
        {
            var dm = _mapper.Map<ServiceDM>(model);
            _serviceBLC.UpdateS(dm);
            return ServiceOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ServiceOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ServiceOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ServiceOR.BadRequest($"< Элемент по ID:{model.ID} - не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ServiceOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ServiceOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ServiceOR.InternalServerError(ex.Message);
        }
    }

    public ServiceOR RemoveService(string id)
    {
        try
        {
            _serviceBLC.DeleteS(id);
            return ServiceOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ServiceOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ServiceOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ServiceOR.BadRequest($"< Элемент по ID:{id} - не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ServiceOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ServiceOR.InternalServerError(ex.Message);
        }
    }
}