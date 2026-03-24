using BSUcontractmodels.DataModels;

namespace BSUcontractmodels.OfficePackage;

public interface IReportBLC
{
    Task<List<MasterVisitsDM>> GetMasterVisitsAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);

    /// Получить отчёт и отправить его на email
    /// <param name="masterID">ID мастера</param>
    /// <param name="dateFrom">Дата начала периода</param>
    /// <param name="dateTo">Дата конца периода</param>
    /// <param name="toEmail">Email адрес для отправки отчёта</param>
    /// <param name="ct">Токен отмены</param>
    /// <returns>True если успешно отправлено, иначе False</returns>
    Task<bool> GenerateAndSendReportAsync(string masterID, DateTime dateFrom, DateTime dateTo, string toEmail, CancellationToken ct);
}
