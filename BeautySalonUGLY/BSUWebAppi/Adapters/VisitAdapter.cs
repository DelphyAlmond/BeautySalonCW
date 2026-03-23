using AutoMapper;
using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;
using BSUdatabase.DBModels;
using BSUmodels.Exceptions;

namespace BSUWebAppi.Adapters;

public class VisitAdapter : IVisitAdapter
{
    private readonly IVisitBLC _visitBLC;
    private readonly ILogger<VisitAdapter> _logger;
    private readonly IMapper _mapper;

    public VisitAdapter(IVisitBLC visitBLC, ILogger<VisitAdapter> logger, IMapper mapper)
    {
        _visitBLC = visitBLC;
        _logger = logger;
        _mapper = mapper;
    }

    private VisitVM BuildVisitVM(VisitDM visitDM)
    {
        var vm = _mapper.Map<VisitVM>(visitDM);
        // [ ! ] _worker, _master и _customer уже в orderDM
        return vm;
    }

    public VisitOR GetElement(string id)
    {
        try
        {
            var visitDM = _visitBLC.GetVisitByData(id);
            var visitVM = BuildVisitVM(visitDM);
            return VisitOR.OK(visitVM);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return VisitOR.NotFound($"< Посещение по ID: {id} не найдено >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR RegisterVisit(VisitBM model)
    {
        try
        {
            var visitDM = _mapper.Map<VisitDM>(model);
            _visitBLC.InsertV(visitDM);
            return VisitOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return VisitOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return VisitOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR UpdateVisit(VisitBM model)
    {
        try
        {
            var visitDM = _mapper.Map<VisitDM>(model);
            _visitBLC.UpdateV(visitDM);
            return VisitOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return VisitOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return VisitOR.BadRequest($"< Посещение по ID:{model.ID} не найдено");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return VisitOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR RemoveVisit(string id)
    {
        try
        {
            _visitBLC.DeleteV(id);
            return VisitOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return VisitOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return VisitOR.BadRequest($"< Посещение по ID:{id} не найдено");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR GetVisitsByDateGap(DateTime from, DateTime to)
    {
        try
        {
            var visits = _visitBLC.GetAllVisitsByDateGap(from, to);
            var vms = visits.Select(v => BuildVisitVM(v)).ToList();
            return VisitOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return VisitOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR GetVisitsByMaster(string workerId, DateTime from, DateTime to)
    {
        try
        {
            var visits = _visitBLC.GetAllVisitsByMaster(workerId, from, to);
            var vms = visits.Select(v => BuildVisitVM(v)).ToList();
            return VisitOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return VisitOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR GetVisitsByCustomer(string customerId, DateTime from, DateTime to)
    {
        try
        {
            var visits = _visitBLC.GetAllVisitsByCustomer(customerId, from, to);
            var vms = visits.Select(v => BuildVisitVM(v)).ToList();
            return VisitOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return VisitOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }

    public VisitOR GetVisitsByService(string serviceId, DateTime from, DateTime to)
    {
        try
        {
            var visits = _visitBLC.GetAllVisitsByService(serviceId, from, to);
            var vms = visits.Select(v => BuildVisitVM(v)).ToList();
            return VisitOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return VisitOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return VisitOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return VisitOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return VisitOR.InternalServerError(ex.Message);
        }
    }
}
