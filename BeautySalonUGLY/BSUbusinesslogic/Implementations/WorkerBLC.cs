using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class WorkerBLC : IWorkerBLC
{
    private readonly IWorkerSC _workerSC;

    public WorkerBLC(IWorkerSC workerSC)
    {
        _workerSC = workerSC;
    }

    public List<WorkerDM> GetAllWorkers(bool onlyActive = true)
    {
        var all = _workerSC.GetWorkers();
        if (onlyActive)
            return all.Where(w => !w.IsDeleted).ToList();
        return all;
    }

    // Без реализации на стороне StorageContract-а [ * ]

    public List<WorkerDM> GetAllWorkersByPost(Post postType, bool onlyActive = true)
    {
        var all = _workerSC.GetWorkers();
        var filtered = all.Where(w => w.PostType == postType);
        if (onlyActive)
            filtered = filtered.Where(w => !w.IsDeleted);
        return filtered.ToList();
    }

    public List<WorkerDM> GetAllWorkersByBDate(DateTime start, DateTime end, bool onlyActive = true)
    {
        // [*] даты к компоненте Date для корректного сравнения
        var startDate = start.Date;
        var endDate = end.Date;

        var all = _workerSC.GetWorkers();
        var filtered = all.Where(w => w.BirthDate.Date >= startDate && w.BirthDate.Date <= endDate);
        if (onlyActive)
            filtered = filtered.Where(w => !w.IsDeleted);
        return filtered.ToList();
    }

    public WorkerDM GetWorkerByData(string data)
    {
        if (data.IsEmpty())
            throw new ValidationException("< Worker BLC: search data is empty >");

        if (data.IsGuid())
        {
            var byId = _workerSC.GetWByID(data);
            if (byId != null) return byId;
        }

        var byName = _workerSC.GetWByName(data);
        if (byName != null) return byName;

        throw new ValidationException($"< Worker with data '{data}' not found >");
    }

    public void InsertW(WorkerDM worker)
    {
        worker.Validate();
        _workerSC.AddW(worker);
    }

    public void UpdateW(WorkerDM worker)
    {
        worker.Validate();
        _workerSC.UpdW(worker);
    }

    public void DeleteW(string id)
    {
        if (id.IsEmpty())
            throw new ValidationException("< Worker BLC: ID for deletion is empty >");
        _workerSC.DelW(id);
    }
}