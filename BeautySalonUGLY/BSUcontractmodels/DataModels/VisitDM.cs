using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class VisitDM(string id, string customerID, string? orderID, string? workerID, string? staffID,
    DateTime plannedDate, List<ServUnitVisitLinkDM> list, double bonuses, bool isCaneled) : IValidation
{
    // similar to\аналогично Order- Data модели:
    private readonly CustomerDM? _customer;
    private readonly WorkerDM? _master;
    private readonly WorkerDM? _worker;
    public string? CustomerName => _customer?.Username;
    public string? MasterName => _master?.FullName;
    public string? WorkerName => _worker?.FullName;

    private double? _summ;
    public string ID { get; private set; } = id;
    public string CustomerID { get; private set; } = customerID;

    // К посещениям должна быть возможность привязывать также и товарооборот (cart rom order),
    // т.к., если в течении услуги понадобится какое-то средство - оно спишется
    // и добавится в общ. стоимость
    public string? OrderID { get; private set; } = orderID;
    // Добавлениие будет происходить в единую корзину-список в объекте заказа (= выдачи)

    // Привлечённый к работе (для исп. услуги в салоне) мастер
    public string WorkerID { get; private set; } = workerID;

    // [ REWISED ] changed
    public string MasterID { get; private set; } = staffID;

    public DateTime DateReg { get; private set; } = DateTime.UtcNow;
    public DateTime DatePlanned { get; private set; } = plannedDate;
    public List<ServUnitVisitLinkDM> Services { get; private set; } = list; // [ ! ] corrected
    
    // public double Summ { get; private set; } = summ; >>
    // Если _summ задан (при маппинге), возвращаем его, иначе вычисляем из Services и Discount
    public double Summ
    {
        get => _summ ?? Math.Max(0, (Services?.Sum(x => x.Price * x.Count) ?? 0) - Discount);
        private set => _summ = value;
    }

    // Дополнительный конструктор для маппинга/слоёв хранения (вызывает primary-конструктор)
    public VisitDM(string id, string customerID, string? orderID, string? workerID, string? staffID,
        DateTime plannedDate, List<ServUnitVisitLinkDM> list, double bonuses, bool isCaneled,
        double summ, WorkerDM worker, WorkerDM master, CustomerDM customer) :
        this(id, customerID, orderID, workerID, staffID, plannedDate, list, bonuses, isCaneled)
    {
        Summ = summ;
        _worker = worker;
        _customer = customer;
        _master = master;
    }

    public VisitDM(string id, string customerID, string? orderID, string? workerID, string? staffID,
        DateTime plannedDate, List<ServUnitVisitLinkDM> list, double bonuses)
        : this(id, customerID, orderID, workerID, staffID, plannedDate, list, bonuses, false) { }

    public double Discount { get; private set; } = bonuses;
    public bool IsCanceled { get; private set; } = isCaneled;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Order: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Order: ID is not valid, not GUID >");
        if (!CustomerID?.IsGuid() ?? !CustomerID?.IsEmpty() ?? false)
            throw new ValidationException("< Order: Customer ID is not valid >");
        if (WorkerID.IsEmpty()) throw new ValidationException("< Order: Worker ID is empty >");
        if (!WorkerID.IsGuid()) throw new ValidationException("< Order: Worker ID is not valid, not GUID >");
        // >>
        if (!MasterID?.IsGuid() ?? !MasterID?.IsEmpty() ?? false)
            throw new ValidationException("< Order: Master ID is not valid >");

        if ((Services?.Count ?? 0) == 0) throw new ValidationException("< Order: Cart is empty, no products chosen >");

        if (Summ <= 0) throw new ValidationException("< Order: Summ less or equal to 0, no products chosen >");
    }
}
