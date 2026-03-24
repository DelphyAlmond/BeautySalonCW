using BSUcontractmodels.AdapterContracts.OperationResponses;

namespace BSUcontractmodels.AdapterContracts;

public interface IReportAdapter
{
    Task<ReportOR> GetMasterVisitsReportAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);

    /// Получить отчёт по посещениям мастера в формате Word документа
    Task<(Stream stream, string fileName)> GetMasterVisitsReportWordAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);
}
