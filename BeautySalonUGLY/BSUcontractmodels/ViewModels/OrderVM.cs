using BSUcontrmodels.Enums;

namespace BSUcontractmodels.ViewModels;

public class OrderVM
{
    // View Model предназначена для отображения данных клиенту(в ответе HTTP).
    // Она содержит только те поля, которые нужно вернуть, и часто их формат
    // отличается от Data Model(например, объединение нескольких полей, вычисляемые свойства).

    // Адаптер или контроллер преобразует Data Model (или набор Data Model-е) в одну
    // или несколько View Models и возвращает их клиенту. В вашем коде OperaionRes.-se
    // использует CustomerVM для формирования успешного ответа.

    public required string ID { get; set; }
    public required string CustomerID { get; set; }
    public string? CustomerName { get; set; }      // денормализовано для клиента
    public required string WorkerID { get; set; }
    public string? WorkerName { get; set; }
    public required DateTime Date { get; set; }
    public required List<ProdUnitOrderLinkVM> Cart { get; set; }
    public required double Summ { get; set; }
    public required double Discount { get; set; }
    public required string Status { get; set; } // enum allows *
}
