using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;
public interface IVisitAdapter
{
    // CRUD
    VisitOR GetElement(string id);           // → IVisitBLC.GetVisitByData
    VisitOR RegisterVisit(VisitBM model);  // → InsertV
    VisitOR UpdateVisit(VisitBM model);    // → UpdateV
    VisitOR RemoveVisit(string id);        // → DeleteV

    // Специализированные запросы
    VisitOR GetVisitsByDateGap(DateTime from, DateTime to);                     // → GetAllVisitsByDateGap
    VisitOR GetVisitsByMaster(string workerId, DateTime from, DateTime to);     // → GetAllVisitsByMaster
    VisitOR GetVisitsByCustomer(string customerId, DateTime from, DateTime to); // → GetAllVisitsByCustomer
    VisitOR GetVisitsByService(string serviceId, DateTime from, DateTime to);   // → GetAllVisitsByService
}