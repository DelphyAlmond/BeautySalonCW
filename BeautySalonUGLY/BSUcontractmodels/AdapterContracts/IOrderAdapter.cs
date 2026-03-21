using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IOrderAdapter
{
    // CRUD
    OrderOR GetElement(string id);        // → IOrderBLC.GetOrderByData
    OrderOR RegisterOrder(OrderBM model); // → InsertO
    OrderOR UpdateOrder(OrderBM model);   // → UpdateO
    OrderOR RemoveOrder(string id);       // → DeleteO

    // Специализированные запросы
    OrderOR GetOrdersByDateGap(DateTime from, DateTime to);                     // → GetAllOrdersByDateGap
    OrderOR GetOrdersByWorker(string workerId, DateTime from, DateTime to);     // → GetAllOrdersByWorker
    OrderOR GetOrdersByCustomer(string customerId, DateTime from, DateTime to); // → GetAllOrdersByCustomer
    OrderOR GetOrdersByProduct(string productId, DateTime from, DateTime to);   // → GetAllOrdersByProduct

    // 1. Преобразовать BM в DM (Data Model) (при этом списки ProdUnitBM превращаются в List<ProdUnitOrderLinkDM>)
    // 2. Вызвать соответствующий метод BLC(например, _orderBLC.InsertO(orderDM))
    // 3. Получить результат(Data Model или список) от BLC
    // 4. Преобразовать DM в VM (View Model), подтянув дополнительные данные
    // (имена, цены) через вызовы к другим BLC или через заранее загруженные данные
    // 5. Упаковать результат в OrderOR (наследник OperationResponse).
    // [ * ] Базовый класс OperationResponse инкапсулирует HTTP‑статус и полезную нагрузку (Result).
}