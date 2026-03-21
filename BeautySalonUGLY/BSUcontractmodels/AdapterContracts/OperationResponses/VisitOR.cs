using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class VisitOR : OperationResponse
{
    public static VisitOR OK(List<VisitVM> data) => OK<VisitOR, List<VisitVM>>(data);
    public static VisitOR OK(VisitVM data) => OK<VisitOR, VisitVM>(data);
    public static VisitOR NoContent() => NoContent<VisitOR>();
    public static VisitOR NotFound(string message) => NotFound<VisitOR>(message);
    public static VisitOR BadRequest(string message) => BadRequest<VisitOR>(message);
    public static VisitOR InternalServerError(string message) => InternalServerError<VisitOR>(message);
}