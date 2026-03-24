using BSUcontractmodels.DataModels;

namespace BSUcontractmodels.OfficePackage;

public interface IReportBLC
{
    Task<List<MasterVisitsDM>> GetMasterVisitsAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct);
}
