using BSUcontractmodels.AdapterContract.OperationResponses;
using BSUcontractmodels.BindingModels;
using BSUcontrmodels.Enums;

namespace BSUcontractmodels.AdapterContracts;

/*
public interface IWorkerAdapter
{
    // > Получить всех работников (опционально только активных)
    WorkerOR GetList(bool onlyActive = true);

    // > Получить работников по должности (опционально только активных)
    WorkerOR GetByPost(Post postType, bool onlyActive = true);

    // > Получить работников по диапазону дат рождения (опционально только активных)
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