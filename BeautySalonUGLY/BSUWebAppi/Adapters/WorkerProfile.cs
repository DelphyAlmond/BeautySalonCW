using AutoMapper;
using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using BSUmodels.Exceptions;

namespace BSUWebAppi.Adapters;

public class WorkerAdapter : IWorkerAdapter
{
    private readonly IWorkerBLC _workerBLC;
    private readonly ILogger<WorkerAdapter> _logger;
    private readonly IMapper _mapper;

    public WorkerAdapter(IWorkerBLC workerBLC, ILogger<WorkerAdapter> logger, IMapper mapper)
    {
        _workerBLC = workerBLC;
        _logger = logger;
        _mapper = mapper;
    }

    public WorkerOR GetList()
    {
        try
        {
            var workers = _workerBLC.GetAllWorkers();
            var vms = workers.Select(x => _mapper.Map<WorkerVM>(x)).ToList();
            return WorkerOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return WorkerOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR GetByPost(Post postType, bool onlyActive = true)
    {
        try
        {
            var workers = _workerBLC.GetAllWorkersByPost(postType, onlyActive);
            var vms = workers.Select(x => _mapper.Map<WorkerVM>(x)).ToList();
            return WorkerOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return WorkerOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR GetByBirthDate(DateTime start, DateTime end, bool onlyActive = true)
    {
        try
        {
            var workers = _workerBLC.GetAllWorkersByBDate(start, end, onlyActive);
            var vms = workers.Select(x => _mapper.Map<WorkerVM>(x)).ToList();
            return WorkerOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return WorkerOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR GetElement(string id)
    {
        try
        {
            var worker = _workerBLC.GetWorkerByData(id);
            return WorkerOR.OK(_mapper.Map<WorkerVM>(worker));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOR.NotFound($"< Элемент по ID: {id} - не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR RegisterWorker(WorkerBM model)
    {
        try
        {
            var dm = _mapper.Map<WorkerDM>(model);
            _workerBLC.InsertW(dm);
            return WorkerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR ChangeWorkerInfo(WorkerBM model)
    {
        try
        {
            var dm = _mapper.Map<WorkerDM>(model);
            _workerBLC.UpdateW(dm);
            return WorkerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOR.BadRequest($"< Элемент по ID:{model.ID} - не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return WorkerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }

    public WorkerOR RemoveWorker(string id)
    {
        try
        {
            _workerBLC.DeleteW(id);
            return WorkerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return WorkerOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return WorkerOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return WorkerOR.BadRequest($"< Элемент по ID:{id} - не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return WorkerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return WorkerOR.InternalServerError(ex.Message);
        }
    }
}
