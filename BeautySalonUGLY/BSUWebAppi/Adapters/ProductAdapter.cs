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

public class ProductAdapter : IProductAdapter
{
    private readonly IProductBLC _productBLC;
    private readonly ILogger<ProductAdapter> _logger;
    private readonly IMapper _mapper;

    public ProductAdapter(IProductBLC productBLC, ILogger<ProductAdapter> logger, IMapper mapper)
    {
        _productBLC = productBLC;
        _logger = logger;
        _mapper = mapper;
    }

    public ProductOR GetList()
    {
        try
        {
            var products = _productBLC.GetAllProducts(false);
            var vms = products.Select(x => _mapper.Map<ProductVM>(x)).ToList();
            return ProductOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }

    public ProductOR GetElement(string id)
    {
        try
        {
            var product = _productBLC.GetProductByData(id);
            return ProductOR.OK(_mapper.Map<ProductVM>(product));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOR.NotFound($"< Элемент по ID: {id} - не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }

    // [ + ] получение продуктов по производителю
    public ProductOR GetByManufacturer(string manufacturerId)
    {
        try
        {
            var products = _productBLC.GetAllProductsByManufacturer(manufacturerId, false);
            var vms = products.Select(x => _mapper.Map<ProductVM>(x)).ToList();
            return ProductOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ProductOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }

    public ProductOR RegisterProduct(ProductBM model)
    {
        try
        {
            var dm = _mapper.Map<ProductDM>(model);
            _productBLC.InsertPitem(dm);
            return ProductOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }

    public ProductOR ChangeProductInfo(ProductBM model)
    {
        try
        {
            var dm = _mapper.Map<ProductDM>(model);
            _productBLC.UpdatePitem(dm);
            return ProductOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOR.BadRequest($"< Элемент по ID:{model.ID} - не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return ProductOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }

    public ProductOR RemoveProduct(string id)
    {
        try
        {
            _productBLC.DeletePitem(id);
            return ProductOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ProductOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ProductOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ProductOR.BadRequest($"< Элемент по ID:{id} - не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ProductOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ProductOR.InternalServerError(ex.Message);
        }
    }
}