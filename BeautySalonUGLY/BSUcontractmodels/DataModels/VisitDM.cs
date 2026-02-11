using BSUcontrmodels.Enums;
using BSUcontrmodels.Infrastructure;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUcontrmodels.DataModels;

public class VisitDM(string id, string customerID, string? orderID, string? workerID, DateTime plannedDate,
    List<ProdUnitOrderLinkDM> list, double summ, double bonuses, bool isCaneled) : IValidation
{
    public string ID { get; private set; } = id;
    public string CustomerID { get; private set; } = customerID;

    // К посещениям должна быть возможность привязывать также и товарооборот (cart rom order),
    // т.к., если в течении услуги понадобится какое-то средство - оно спишется
    // и добавится в общ. стоимость
    public string? OrderID { get; private set; } = orderID;
    // Добавлениие будет происходить в единую корзину-список в объекте заказа (= выдачи)

    // Привлечённый к работе (для исп. услуги в салоне) мастер
    public string? WorkerID { get; private set; } = workerID;
    public DateTime DateReg { get; private set; } = DateTime.UtcNow;
    public DateTime DatePlanned { get; private set; } = plannedDate;
    public List<ProdUnitOrderLinkDM> Services { get; private set; } = list;
    public double Summ { get; private set; } = summ; // + order summ
    public double Discount { get; private set; } = bonuses;
    public bool IsCanceled { get; private set; } = isCaneled;

    public void Validate()
    {
        if (ID.IsEmpty()) throw new ValidationException("< Order: ID is empty >");
        if (!ID.IsGuid()) throw new ValidationException("< Order: ID is not valid, not GUID >");
        if (!CustomerID?.IsGuid() ?? !CustomerID?.IsEmpty() ?? false) throw new ValidationException("< Order: Customer ID is not valid >");
        if (WorkerID.IsEmpty()) throw new ValidationException("< Order: Worker ID is empty >");
        if (!WorkerID.IsGuid()) throw new ValidationException("< Order: Worker ID is not valid, not GUID >");

        if ((Services?.Count ?? 0) == 0) throw new ValidationException("< Order: Cart is empty, no products chosen >");

        if (Summ <= 0) throw new ValidationException("< Order: Summ less or equal to 0, no products chosen >");
    }
}
