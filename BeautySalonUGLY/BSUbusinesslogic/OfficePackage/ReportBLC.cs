using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.DataModels;
using BSUcontractmodels.OfficePackage;
using BSUcontractmodels.Exceptions;
using BSUmodels.Extensions;
using BSUmodels.Exceptions;
using BSUcontrmodels.DataModels;

namespace BSUbusinesslogic.OfficePackage;

public class ReportBLC(IVisitBLC visitBLC, IWorkerBLC workerBLC) : IReportBLC
{
    private readonly IVisitBLC _visitBLC = visitBLC;
    private readonly IWorkerBLC _workerBLC = workerBLC;

    /// Получить список посещений конкретного мастера с её детальной информацией
    public async Task<List<MasterVisitsDM>> GetMasterVisitsAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct)
    {
        // Валидация входных параметров
        if (masterID.IsEmpty())
            throw new ValidationException("< Report BLC: MasterID is empty >");

        if (!masterID.IsGuid())
            throw new ValidationException("< Report BLC: MasterID is not valid GUID >");

        if (dateFrom > dateTo)
            throw new ValidationException("< Report BLC: Start date cannot be after end date >");

        // Получаем данные о мастере
        var master = _workerBLC.GetWorkerByData(masterID);
        if (master == null)
            throw new ElementNotFoundException(nameof(WorkerDM), masterID);

        // Получаем все посещения мастера за указанный период
        var visits = await Task.Run(() => 
            _visitBLC.GetAllVisitsByMaster(masterID, dateFrom, dateTo), ct);

        if (visits == null || visits.Count == 0)
            throw new NullListException();

        // Формируем описание посещений
        var visitDescriptions = visits
            .OrderBy(v => v.DatePlanned)
            .Select(v => FormatVisitDescription(v))
            .ToList();

        // Создаём и возвращаем результат
        var result = new List<MasterVisitsDM>
        {
            new MasterVisitsDM
            {
                MasterFullName = master.FullName,
                Visits = visitDescriptions
            }
        };

        return result;
    }

    /// Форматирует описание посещения в строку для отчёта
    private static string FormatVisitDescription(VisitDM visit)
    {
        var date = visit.DatePlanned.ToString("dd.MM.yyyy HH:mm");
        var customer = visit.CustomerName ?? "Неизвестный клиент";
        var services = visit.Services?.Count > 0 ? $"{visit.Services.Count} услуг(и)" : "Услуги не указаны";
        var summ = visit.Summ.ToString("F2");

        return $"{date} | {customer} | {services} | {summ} руб.";
    }
}
