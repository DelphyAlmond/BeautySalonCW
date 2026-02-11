using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class OrderBLC : IOrderBLC
{
    private readonly IOrderSC _orderSC;

    public OrderBLC(IOrderSC orderSC)
    {
        _orderSC = orderSC;
    }

    // все заказы за указанный период (без дополнительных фильтров)
    public List<OrderDM> GetAllOrdersByDateGap(DateTime from, DateTime to)
    {
        return _orderSC.GetOrdersByDateGap(from, to, null, null, null);
    }

    // заказы конкретного сотрудника (worker) за период
    public List<OrderDM> GetAllOrdersByWorker(string workerID, DateTime from, DateTime to)
    {
        if (workerID.IsEmpty())
            throw new ValidationException("< Order BLC: WorkerID is empty >");
        if (!workerID.IsGuid())
            throw new ValidationException("< Order BLC: WorkerID is not valid GUID >");

        return _orderSC.GetOrdersByDateGap(from, to, workerID, null, null);
    }

    // заказы конкретного клиента за период
    public List<OrderDM> GetAllOrdersByCustomer(string customerID, DateTime from, DateTime to)
    {
        if (customerID.IsEmpty())
            throw new ValidationException("< Order BLC: CustomerID is empty >");
        if (!customerID.IsGuid())
            throw new ValidationException("< Order BLC: CustomerID is not valid GUID >");

        return _orderSC.GetOrdersByDateGap(from, to, null, customerID, null);
    }

    // содержащие конкретный товар заказы, за период
    public List<OrderDM> GetAllOrdersByProduct(string productID, DateTime from, DateTime to)
    {
        if (productID.IsEmpty())
            throw new ValidationException("< Order BLC: ProductID is empty >");
        if (!productID.IsGuid())
            throw new ValidationException("< Order BLC: ProductID is not valid GUID >");

        return _orderSC.GetOrdersByDateGap(from, to, null, null, productID);
    }

    public OrderDM GetOrderByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Order BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _orderSC.GetOByID(data);
            if (byId != null) return byId;
        }

        throw new ValidationException($"< Order with data '{data}' not found >");
    }

    public void InsertO(OrderDM orderDM)
    {
        orderDM.Validate();
        _orderSC.AddO(orderDM);
    }

    public void UpdateO(OrderDM orderDM)
    {
        orderDM.Validate();
        _orderSC.UpdO(orderDM);
    }

    public void DeleteO(string id)
    {
        if (id.IsEmpty())
            throw new ValidationException("< Order BLC: ID for deletion is empty >");
        if (!id.IsGuid())
            throw new ValidationException("< Order BLC: ID is not valid GUID >");

        _orderSC.DelO(id);
    }
}