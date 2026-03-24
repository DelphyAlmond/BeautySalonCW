using BSUcontractmodels.AdapterContracts.OperationResponses;

namespace BSUcontractmodels.AdapterContracts;

public interface IReportAdapter
{
    /// Получить отчёт по посещениям мастера (JSON)
    Task<ReportOR> GetMasterVisitsReportAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);

    /// Получить отчёт по посещениям мастера в формате Word документа (.docx)
    Task<ReportOR> GetMasterVisitsReportWordAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);

    /// Сформировать отчёт и отправить его на email
    /// ID мастера, Дата начала периода, Дата конца периода, Email адрес получателя, Токен отмены
    /// На выходе : OperationResponse с статусом отправки
    Task<ReportOR> GenerateAndSendReportViaEmailAsync(string masterID, DateTime dateFrom, DateTime dateTo, string toEmail, CancellationToken ct);
}
