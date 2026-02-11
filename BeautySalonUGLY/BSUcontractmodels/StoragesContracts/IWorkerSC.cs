using BSUcontrmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

public interface IWorkerSC
{
    List<WorkerDM> GetWorkers();
    WorkerDM? GetWByID(string id);
    WorkerDM? GetWByName(string fullname);

    // [ * ]

    void AddW(WorkerDM worker);
    void UpdW(WorkerDM worker);
    void DelW(string id);
}
