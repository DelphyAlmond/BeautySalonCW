using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IVisitSC
{
    List<VisitDM> GetVisitsByDateGap(DateTime start, DateTime end, string? masterID, string? customerID, string? serviceID);
    VisitDM? GetVByID(string id);

    void AddV(VisitDM visit);
    void UpdV(VisitDM visit);
    void DelV(string id);
}
