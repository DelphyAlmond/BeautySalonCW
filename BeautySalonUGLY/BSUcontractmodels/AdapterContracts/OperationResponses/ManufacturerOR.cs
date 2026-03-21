using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContracts.OperationResponses;

public class ManufacturerOR : OperationResponse
{
    public static ManufacturerOR OK(List<ManufacturerVM> data) => OK<ManufacturerOR, List<ManufacturerVM>>(data);
    public static ManufacturerOR OK(ManufacturerVM data) => OK<ManufacturerOR, ManufacturerVM>(data);
    public static ManufacturerOR NoContent() => NoContent<ManufacturerOR>();
    public static ManufacturerOR NotFound(string message) => NotFound<ManufacturerOR>(message);
    public static ManufacturerOR BadRequest(string message) => BadRequest<ManufacturerOR>(message);
    public static ManufacturerOR InternalServerError(string message) => InternalServerError<ManufacturerOR>(message);
}