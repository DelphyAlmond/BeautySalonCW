using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class OrderOR : OperationResponse
{
    public static OrderOR OK(List<OrderVM> data) => OK<OrderOR, List<OrderVM>>(data);
    public static OrderOR OK(OrderVM data) => OK<OrderOR, OrderVM>(data);
    public static OrderOR NoContent() => NoContent<OrderOR>();
    public static OrderOR NotFound(string message) => NotFound<OrderOR>(message);
    public static OrderOR BadRequest(string message) => BadRequest<OrderOR>(message);
    public static OrderOR InternalServerError(string message) => InternalServerError<OrderOR>(message);
}