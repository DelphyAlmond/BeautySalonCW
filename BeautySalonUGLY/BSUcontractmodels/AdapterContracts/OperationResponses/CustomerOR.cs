using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class CustomerOR : OperationResponse
{
    public static CustomerOR OK(List<CustomerVM> data) => OK<CustomerOR, List<CustomerVM>>(data);
    public static CustomerOR OK(CustomerVM data) => OK<CustomerOR, CustomerVM>(data);
    public static CustomerOR NoContent() => NoContent<CustomerOR>();
    public static CustomerOR NotFound(string message) => NotFound<CustomerOR>(message);
    public static CustomerOR BadRequest(string message) => BadRequest<CustomerOR>(message);
    public static CustomerOR InternalServerError(string message) => InternalServerError<CustomerOR>(message);
}
