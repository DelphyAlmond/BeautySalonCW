using AutoMapper;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.AdapterContracts.OperationResponses;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;

namespace BSUWebAppi.Adapters;

public class ManufacturerAdapter : IManufacturerAdapter
{
    private readonly IManufacturerBLC _manufacturerBLC;
    private readonly ILogger<ManufacturerAdapter> _logger;
    private readonly IMapper _mapper;

    public ManufacturerAdapter(IManufacturerBLC manufacturerBLC, ILogger<ManufacturerAdapter> logger, IMapper mapper)
    {
        _manufacturerBLC = manufacturerBLC;
        _logger = logger;
        _mapper = mapper;
    }

    public ManufacturerOR GetList()
    {
        try
        {
            var manufacturers = _manufacturerBLC.GetAllManufacturers();
            var vms = manufacturers.Select(x => _mapper.Map<ManufacturerVM>(x)).ToList();
            return ManufacturerOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ManufacturerOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ManufacturerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ManufacturerOR.InternalServerError(ex.Message);
        }
    }

    public ManufacturerOR GetElement(string id)
    {
        try
        {
            var manufacturer = _manufacturerBLC.GetManufacturersByData(id);
            return ManufacturerOR.OK(_mapper.Map<ManufacturerVM>(manufacturer));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ManufacturerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ManufacturerOR.NotFound($"< Элемент по ID: {id} - не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ManufacturerOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ManufacturerOR.InternalServerError(ex.Message);
        }
    }

    public ManufacturerOR RegisterManufacturer(ManufacturerBM model)
    {
        try
        {
            var dm = _mapper.Map<ManufacturerDM>(model);
            _manufacturerBLC.InsertM(dm);
            return ManufacturerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ManufacturerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ManufacturerOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ManufacturerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ManufacturerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ManufacturerOR.InternalServerError(ex.Message);
        }
    }

    public ManufacturerOR ChangeManufacturerInfo(ManufacturerBM model)
    {
        try
        {
            var dm = _mapper.Map<ManufacturerDM>(model);
            _manufacturerBLC.UpdateM(dm);
            return ManufacturerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ManufacturerOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ManufacturerOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ManufacturerOR.BadRequest($"< Элемент по ID:{model.ID} - не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ManufacturerOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ManufacturerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ManufacturerOR.InternalServerError(ex.Message);
        }
    }

    public ManufacturerOR RemoveManufacturer(string id)
    {
        try
        {
            _manufacturerBLC.DeleteM(id);
            return ManufacturerOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ManufacturerOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ManufacturerOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ManufacturerOR.BadRequest($"< Элемент по ID:{id} - не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ManufacturerOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ManufacturerOR.InternalServerError(ex.Message);
        }
    }
}