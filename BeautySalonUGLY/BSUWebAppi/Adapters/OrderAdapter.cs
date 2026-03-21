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

public class OrderAdapter : IOrderAdapter
{
    private readonly IOrderBLC _orderBLC;
    private readonly ICustomerBLC _customerBLC;
    private readonly IWorkerBLC _workerBLC;
    private readonly IProductBLC _productBLC;
    private readonly ILogger<OrderAdapter> _logger;
    private readonly IMapper _mapper;

    public OrderAdapter(IOrderBLC orderBLC, ICustomerBLC customerBLC, IWorkerBLC workerBLC,
        IProductBLC productBLC, ILogger<OrderAdapter> logger, IMapper mapper)
    {
        _orderBLC = orderBLC;
        _customerBLC = customerBLC;
        _workerBLC = workerBLC;
        _productBLC = productBLC;
        _logger = logger;
        _mapper = mapper;
    }

    private string? GetCustomerName(string? customerId)
    {
        if (string.IsNullOrEmpty(customerId)) return null;
        try
        {
            return _customerBLC.GetCustomerByData(customerId)?.Username;
        }
        catch { return null; }
    }

    private string? GetWorkerName(string? workerId)
    {
        if (string.IsNullOrEmpty(workerId)) return null;
        try
        {
            return _workerBLC.GetWorkerByData(workerId)?.FullName;
        }
        catch { return null; }
    }

    private OrderVM BuildOrderVM(OrderDM orderDM)
    {
        var vm = _mapper.Map<OrderVM>(orderDM);
        vm.CustomerName = GetCustomerName(orderDM.CustomerID);
        vm.WorkerName = GetWorkerName(orderDM.WorkerID);
        vm.MasterName = GetWorkerName(orderDM.MasterID);

        if (vm.Cart != null)
        {
            foreach (var item in vm.Cart)
            {
                try
                {
                    var product = _productBLC.GetProductByData(item.ProductID);
                    item.ProductName = product?.ProductNaming ?? "Unknown";
                    item.Price = product?.Price ?? 0;
                }
                catch { }
            }
        }
        return vm;
    }

    public OrderOR GetElement(string id)
    {
        try
        {
            var orderDM = _orderBLC.GetOrderByData(id);
            var orderVM = BuildOrderVM(orderDM);
            return OrderOR.OK(orderVM);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return OrderOR.NotFound($"< Заказ по ID: {id} не найден >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR RegisterOrder(OrderBM model)
    {
        try
        {
            var orderDM = _mapper.Map<OrderDM>(model);
            _orderBLC.InsertO(orderDM);
            return OrderOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return OrderOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return OrderOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR UpdateOrder(OrderBM model)
    {
        try
        {
            var orderDM = _mapper.Map<OrderDM>(model);
            _orderBLC.UpdateO(orderDM);
            return OrderOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return OrderOR.BadRequest($"< Ошибка - доставлены неверные данные: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return OrderOR.BadRequest($"< Заказ по ID:{model.ID} не найден");
        }
        catch (ElementExistsException ex)
        {
            _logger.LogError(ex, "ElementExistsException");
            return OrderOR.BadRequest(ex.Message);
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR RemoveOrder(string id)
    {
        try
        {
            _orderBLC.DeleteO(id);
            return OrderOR.NoContent();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - ID пуст >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return OrderOR.BadRequest($"< Ошибка - доставлен неверный ID: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return OrderOR.BadRequest($"< Заказ по ID:{id} не найден");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.BadRequest($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR GetOrdersByDateGap(DateTime from, DateTime to)
    {
        try
        {
            var orders = _orderBLC.GetAllOrdersByDateGap(from, to);
            var vms = orders.Select(o => BuildOrderVM(o)).ToList();
            return OrderOR.OK(vms);
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return OrderOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR GetOrdersByWorker(string workerId, DateTime from, DateTime to)
    {
        try
        {
            var orders = _orderBLC.GetAllOrdersByWorker(workerId, from, to);
            var vms = orders.Select(o => BuildOrderVM(o)).ToList();
            return OrderOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return OrderOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR GetOrdersByCustomer(string customerId, DateTime from, DateTime to)
    {
        try
        {
            var orders = _orderBLC.GetAllOrdersByCustomer(customerId, from, to);
            var vms = orders.Select(o => BuildOrderVM(o)).ToList();
            return OrderOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return OrderOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }

    public OrderOR GetOrdersByProduct(string productId, DateTime from, DateTime to)
    {
        try
        {
            var orders = _orderBLC.GetAllOrdersByProduct(productId, from, to);
            var vms = orders.Select(o => BuildOrderVM(o)).ToList();
            return OrderOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return OrderOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return OrderOR.NotFound("< Ошибка - не инициализирован >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return OrderOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return OrderOR.InternalServerError(ex.Message);
        }
    }
}
