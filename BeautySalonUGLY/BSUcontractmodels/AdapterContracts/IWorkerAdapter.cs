using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;
using BSUcontrmodels.Enums;

namespace BSUcontractmodels.AdapterContracts;

/*
public interface IWorkerAdapter
{
    WorkerOR GetList();
    WorkerOR GetElement(string id);
    WorkerOR RegisterWorker(WorkerBM model);
    WorkerOR ChangeWorkerInfo(WorkerBM model);
    WorkerOR RemoveWorker(string id);
}
*/

public interface IWorkerAdapter
{
    // > Получить всех работников (опционально, только активных)
    WorkerOR GetList(bool onlyActive = true);

    // > Получить работников по должности (optional : active)
    WorkerOR GetByPost(Post postType, bool onlyActive = true);

    // > Получить работников по диапазону дат рождения (optional : active)
    WorkerOR GetByBirthDate(DateTime start, DateTime end, bool onlyActive = true);

    // > Получить одного работника по идентификатору или другим данным
    WorkerOR GetElement(string data);

    // > Зарегистрировать нового работника
    WorkerOR RegisterWorker(WorkerBM model);

    // > Изменить информацию о работнике
    WorkerOR ChangeWorkerInfo(WorkerBM model);

    // > Удалить работника по идентификатору
    WorkerOR RemoveWorker(string id);
}