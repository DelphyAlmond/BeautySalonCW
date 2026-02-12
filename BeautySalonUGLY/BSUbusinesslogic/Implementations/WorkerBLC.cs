using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using BSUmodels.Exceptions;
using BSUmodels.Extensions;

namespace BSUbusinesslogic.Implementations;

public class WorkerBLC(IWorkerSC workerSC) : IWorkerBLC
{
    private readonly IWorkerSC _workerSC = workerSC;

    public List<WorkerDM> GetAllWorkers(bool onlyActive = true)
    {
        var all = _workerSC.GetWorkers();
        if (onlyActive)
            return all.Where(w => !w.IsDeleted).ToList();
        return all ?? throw new NullListException();
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
            throw new ArgumentNullException($"< Worker BLC: search data - {nameof(data)}, is empty >");

        if (data.IsGuid())
        {
            return _workerSC.GetWByID(data) ?? throw new ElementNotFoundException(null, data);
        }
        return _workerSC.GetWByName(data) ?? throw new ElementNotFoundException($"< Worker with data '{data}' not found >", data);
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
        if (id.IsEmpty() || !id.IsGuid())
            throw new ValidationException("< Worker BLC: ID for deletion is empty or not valid>");
        _workerSC.DelW(id);
    }
}