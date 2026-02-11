using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IVisitSC
{
    // List<OrderDM> GetVisits(); - все посещения тоже, возможно, не пригодятся, только по диапазону:
    List<VisitDM> GetVisitsByDateGap(DateTime start, DateTime end, string? masterID, string? customerID);
    // List<VisitDM> GetMasterVisits(string masterID); // все записи к данному мастеру
    // List<VisitDM> GetCustomerVisits(string customerID); // объединено в ByDateGap выше ^
    VisitDM? GetVByID(string id);

    void AddV(VisitDM visit);
    void UpdV(VisitDM visit);
    void DelV(string id);
}
