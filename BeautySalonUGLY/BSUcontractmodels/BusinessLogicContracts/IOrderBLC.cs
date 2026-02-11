using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IOrderBLC
{
    List<OrderDM> GetAllOrdersByDateGap(DateTime from, DateTime to);
    List<OrderDM> GetAllOrdersByWorker(string  workerID, DateTime from, DateTime to);
    List<OrderDM> GetAllOrdersByCustomer(string customerID, DateTime from, DateTime to);
    List<OrderDM> GetAllOrdersByProduct(string productID, DateTime from, DateTime to);
    OrderDM GetOrderByData(string data);
    void InsertO(OrderDM orderDM);
    void UpdateO(OrderDM orderDM);
    void DeleteO(string id);
}
