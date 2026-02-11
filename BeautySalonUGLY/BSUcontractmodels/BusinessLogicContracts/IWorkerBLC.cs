using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;

namespace BSUcontractmodels.BusinessLogicContracts;

internal interface IWorkerBLC
{
    List<WorkerDM> GetAllWorkers(bool onlyActive = true);
    List<WorkerDM> GetAllWorkersByPost(Post postType, bool onlyActive = true);
    List<WorkerDM> GetAllWorkersByBDate(DateTime start, DateTime end, bool onlyActive = true);
    List<WorkerDM> GetAllWorkersByEmplDate(DateTime start, DateTime end, bool onlyActive = true);
    WorkerDM GetWorkerByData(string data);
    void InsertW(WorkerDM worker);
    void UpdateW(WorkerDM worker);
    void DeleteW(string id);
}
