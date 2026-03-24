namespace BSUcontractmodels.OfficePackage;

/// Сервис для отправки отчётов по электронной почте
public interface IEmailSenderBLC
{
    Task<bool> SendMasterVisitReportAsync(
        string toEmail,
        string masterName,
        Stream reportStream,
        string fileName,
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken ct);
}
