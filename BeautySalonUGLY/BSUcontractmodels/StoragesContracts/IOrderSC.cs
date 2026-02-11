using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IOrderSC
{
    // List<OrderDM> GetOrders(); - все продажи - маловероятно, вместо этого по диапазону:
    List<OrderDM> GetOrdersByDateGap(DateTime start, DateTime end, string? productID);
                                                                                  // сколько раз товар X был
                                                                                  // продан за это время
    List<OrderDM> GetWorkerOrders(DateTime start, DateTime end, string workerID);
    List<OrderDM> GetCustomerOrders(string? customerID); // не предполагает загруженность по кол-ву записей
    OrderDM? GetOByID(string id);

    void AddO(OrderDM order);
    void UpdO(OrderDM order);
    void DelO(string id);
}
