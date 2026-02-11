using BSUcontractmodels.BusinessLogicContracts;
using BSUcontrmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class VisitBLC : IVisitBLC
{
    private readonly IVisitSC _visitSC;

    public VisitBLC(IVisitSC visitSC)
    {
        _visitSC = visitSC;
    }

    public List<VisitDM> GetAllVisitsByDateGap(DateTime from, DateTime to)
    {
        return _visitSC.GetVisitsByDateGap(from, to, null, null, null);
    }

    public List<VisitDM> GetAllVisitsByMaster(string workerID, DateTime from, DateTime to)
    {
        if (workerID.IsEmpty())
            throw new ValidationException("< Order BLC: WorkerID is empty >");
        if (!workerID.IsGuid())
            throw new ValidationException("< Order BLC: WorkerID is not valid GUID >");

        return _visitSC.GetVisitsByDateGap(from, to, workerID, null, null);
    }

    public List<VisitDM> GetAllVisitsByCustomer(string customerID, DateTime from, DateTime to)
    {
        if (customerID.IsEmpty())
            throw new ValidationException("< Order BLC: CustomerID is empty >");
        if (!customerID.IsGuid())
            throw new ValidationException("< Order BLC: CustomerID is not valid GUID >");

        return _visitSC.GetVisitsByDateGap(from, to, null, customerID, null);
    }

    public List<VisitDM> GetAllVisitsByService(string serviceID, DateTime from, DateTime to)
    {
        if (serviceID.IsEmpty())
            throw new ValidationException("< Visit BLC: ProductID is empty >");
        if (!serviceID.IsGuid())
            throw new ValidationException("< Visit BLC: ProductID is not valid GUID >");

        return _visitSC.GetVisitsByDateGap(from, to, null, null, serviceID);
    }

    public VisitDM GetVisitByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Visit BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _visitSC.GetVByID(data);
            if (byId != null) return byId;
        }

        throw new ValidationException($"< Visit with data '{data}' not found >");
    }

    public void InsertV(VisitDM visitDM)
    {
        visitDM.Validate();
        _visitSC.AddV(visitDM);
    }

    public void UpdateV(VisitDM visitDM)
    {
        visitDM.Validate();
        _visitSC.UpdV(visitDM);
    }

    public void DeleteV(string id)
    {
        if (id.IsEmpty())
            throw new ValidationException("< Visit BLC: ID for deletion is empty >");
        _visitSC.DelV(id);
    }
}