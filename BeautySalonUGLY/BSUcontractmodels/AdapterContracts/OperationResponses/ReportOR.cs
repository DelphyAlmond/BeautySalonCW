using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContracts.OperationResponses;

public class ReportOR : OperationResponse
{
    public static ReportOR OK(List<MasterVisitsVM> data) => OK<ReportOR, List<MasterVisitsVM>>(data);
    public static ReportOR OK(MasterVisitsVM data) => OK<ReportOR, MasterVisitsVM>(data);
    public static ReportOR NoContent() => NoContent<ReportOR>();
    public static ReportOR NotFound(string message) => NotFound<ReportOR>(message);
    public static ReportOR BadRequest(string message) => BadRequest<ReportOR>(message);
    public static ReportOR InternalServerError(string message) => InternalServerError<ReportOR>(message);
}
