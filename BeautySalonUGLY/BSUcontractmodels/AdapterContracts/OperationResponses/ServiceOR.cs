using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class ServiceOR : OperationResponse
{
    public static ServiceOR OK(List<ServiceVM> data) => OK<ServiceOR, List<ServiceVM>>(data);
    public static ServiceOR OK(ServiceVM data) => OK<ServiceOR, ServiceVM>(data);
    public static ServiceOR NoContent() => NoContent<ServiceOR>();
    public static ServiceOR NotFound(string message) => NotFound<ServiceOR>(message);
    public static ServiceOR BadRequest(string message) => BadRequest<ServiceOR>(message);
    public static ServiceOR InternalServerError(string message) => InternalServerError<ServiceOR>(message);
}