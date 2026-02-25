using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IWorkerSC
{
    List<WorkerDM> GetWorkers(bool onlyActive = true,
        DateTime? fromBD = null, DateTime? toBD = null,
        DateTime? fromEmploymentDate = null,
        DateTime? toEmploymentDate = null);
    // не было предусмотрено, нужно для мягкого удаления + доп. фильтраций
    WorkerDM? GetWByID(string id);
    WorkerDM? GetWByName(string fullname);

    // [ * ]

    void AddW(WorkerDM worker);
    void UpdW(WorkerDM worker);
    void DelW(string id);
}
