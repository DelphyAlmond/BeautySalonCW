using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;
using System;

namespace BSUcontrmodels.DataModels;

public class OrderDM(string id, string? customerID, string workerID, List<ProdUnitOrderLinkDM> list,
    double bonuses, OrderStatus status) : IValidation
{
    private readonly CustomerDM? _customer;
    private readonly WorkerDM? _worker;
    public string? CustomerName => _customer?.Username;
    public string? WorkerName => _worker?.FullName;

    private double? _summ;

    public string ID { get; private set; } = id;
    public string? CustomerID { get; private set; } = customerID;
    public string WorkerID { get; private set; } = workerID;
    public DateTime Date { get; private set; } = DateTime.UtcNow;

    // Поскольку пользователи имеют возможность возыметь чек на покупаемую косметику,
    // список (корзина cart) продуктов в выдаче (order) и будет представлять из себя чек
    public List<ProdUnitOrderLinkDM>? Cart { get; private set; } = list;
    // Предусмотрена также и выдача материалов мастерам (masterID) услуги, в случае надобности

    public double Discount { get; private set; } = bonuses;

    // Если _summ задан (при маппинге), возвращаем его, иначе вычисляем из Cart и Discount
    public double Summ
    {
        get => _summ ?? Math.Max(0, (Cart?.Sum(x => x.Price * x.Count) ?? 0) - Discount);
        private set => _summ = value;
    }
    // to assign ^ ~ Cart.Sum(x => x.Price * x.Count - Discount) ?? 0;
    public OrderStatus Status { get; private set; } = status;

    // Дополнительный конструктор для маппинга/слоёв хранения (вызывает primary-конструктор)
    public OrderDM(string id, string? customerID, string workerID, List<ProdUnitOrderLinkDM> list,
        double bonuses, OrderStatus status, double summ, WorkerDM worker, CustomerDM customer) :
        this(id, customerID, workerID, list, bonuses, status)
    {
        Summ = summ; // * присвоение разрешено через private set
        _worker = worker;
        _customer = customer;
    }

    public OrderDM(string id, string? customerID, string workerID, List<ProdUnitOrderLinkDM> list,
        double bonuses) : this(id, customerID, workerID, list, bonuses, OrderStatus.Canceled) { }

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Order: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Order: ID is not valid, not GUID >");
        if (!CustomerID?.IsGuid() ?? !CustomerID?.IsEmpty() ?? false)
            throw new ValidationException("< Order: Customer ID is not valid >");
        if (WorkerID.IsEmpty()) throw new ValidationException("< Order: Worker ID is empty >");
        if (!WorkerID.IsGuid()) throw new ValidationException("< Order: Worker ID is not valid, not GUID >");

        if ((Cart?.Count ?? 0) == 0) throw new ValidationException("< Order: Cart is empty, no products chosen >");

        if (Summ <= 0) throw new ValidationException("< Order: Summ less or equal to 0, no products chosen >");
        // < ? >
        if (Status == OrderStatus.Canceled) throw new ValidationException("< Status is not valid, can't be Canceled at first >");
    }
}
