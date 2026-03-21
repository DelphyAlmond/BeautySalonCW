using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class ProductOR : OperationResponse
{
    public static ProductOR OK(List<ProductVM> data) => OK<ProductOR, List<ProductVM>>(data);
    public static ProductOR OK(ProductVM data) => OK<ProductOR, ProductVM>(data);
    public static ProductOR NoContent() => NoContent<ProductOR>();
    public static ProductOR NotFound(string message) => NotFound<ProductOR>(message);
    public static ProductOR BadRequest(string message) => BadRequest<ProductOR>(message);
    public static ProductOR InternalServerError(string message) => InternalServerError<ProductOR>(message);
}