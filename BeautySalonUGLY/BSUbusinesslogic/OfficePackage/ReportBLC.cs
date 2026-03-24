using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.DataModels;
using BSUcontractmodels.OfficePackage;
using BSUcontractmodels.Exceptions;
using BSUmodels.Extensions;
using BSUmodels.Exceptions;
using BSUcontrmodels.DataModels;

namespace BSUbusinesslogic.OfficePackage;

public class ReportBLC(IVisitBLC visitBLC, IWorkerBLC workerBLC, IReportDocumentBLC reportDocBLC, IEmailSenderBLC emailSender) : IReportBLC
{
    private readonly IVisitBLC _visitBLC = visitBLC;
    private readonly IWorkerBLC _workerBLC = workerBLC;
    private readonly IReportDocumentBLC _reportDocBLC = reportDocBLC;
    private readonly IEmailSenderBLC _emailSender = emailSender;

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

        // Получаем все посещения мастера за указанный период - асинхронный [ ! ]
        var visits = await _visitBLC.GetAllVisitsByMasterAsync(masterID, dateFrom, dateTo);

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

    /// Сформировать отчёт и отправить его на email
    public async Task<bool> GenerateAndSendReportAsync(string masterID, DateTime dateFrom, DateTime dateTo, string toEmail, CancellationToken ct)
    {
        // Получаем данные отчёта
        var masterVisits = await GetMasterVisitsAsync(masterID, dateFrom, dateTo, ct);

        if (masterVisits == null || masterVisits.Count == 0)
            throw new NullListException();

        // Формируем документ
        var reportStream = _reportDocBLC.GenerateMasterVisitsWordReportWithDates(masterVisits, dateFrom, dateTo);

        // Получаем данные мастера для темы письма
        var master = _workerBLC.GetWorkerByData(masterID);
        var masterName = master?.FullName ?? "Unknown Master";

        // Формируем имя файла
        var fileName = $"Report_{masterName.Replace(" ", "_")}_{dateFrom:yyyy-MM-dd}_{dateTo:yyyy-MM-dd}.docx";

        // Отправляем отчёт на email
        return await _emailSender.SendMasterVisitReportAsync(
            toEmail,
            masterName,
            reportStream,
            fileName,
            dateFrom,
            dateTo,
            ct);
    }
}
