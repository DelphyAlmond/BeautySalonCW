using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;

namespace BSUcontractmodels.AdapterContracts;

public interface IWorkerAdapter
{
    // same*>
    WorkerOR GetList();
    WorkerOR GetElement(string id);
    WorkerOR RegisterWorker(WorkerBM model);
    WorkerOR ChangeWorkerInfo(WorkerBM model);
    WorkerOR RemoveWorker(string id);
}