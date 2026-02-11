using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

public interface IVisitBLC
{
    List<VisitDM> GetAllVisitsByDateGap(DateTime from, DateTime to);
    List<VisitDM> GetAllVisitsByMaster(string workerID, DateTime from, DateTime to);
    List<VisitDM> GetAllVisitsByCustomer(string customerID, DateTime from, DateTime to);
    List<VisitDM> GetAllVisitsByService(string serviceID, DateTime from, DateTime to);
    VisitDM GetVisitByData(string data);
    void InsertV(VisitDM orderDM);
    void UpdateV(VisitDM orderDM);
    void DeleteV(string id);
}
