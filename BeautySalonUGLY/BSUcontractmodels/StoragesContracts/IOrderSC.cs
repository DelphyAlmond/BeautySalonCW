using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;

namespace BSUcontractmodels.StoragesContracts;

public interface IOrderSC
{
    // List<OrderDM> GetOrders(); - все продажи - маловероятно, вместо этого по диапазону:
    List<OrderDM> GetOrdersByDateGap(DateTime start, DateTime end, string? workerID, string? customerID, string? productID);
                                     // предполагают загруженность по кол-ву записей для "всего"      || сколько раз товар X был
                                                                                                      // продан за это время
    List<OrderDM> GetOrdersByStatus(OrderStatus status, DateTime from, DateTime to);
    OrderDM? GetOByID(string id);

    void AddO(OrderDM order);
    void UpdO(OrderDM order);
    void DelO(string id);
}
