using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.ViewModels;

namespace BSUcontractmodels.AdapterContract.OperationResponses;

public class WorkerOR : OperationResponse
{
    public static WorkerOR OK(List<WorkerVM> data) => OK<WorkerOR, List<WorkerVM>>(data);
    public static WorkerOR OK(WorkerVM data) => OK<WorkerOR, WorkerVM>(data);
    public static WorkerOR NoContent() => NoContent<WorkerOR>();
    public static WorkerOR NotFound(string message) => NotFound<WorkerOR>(message);
    public static WorkerOR BadRequest(string message) => BadRequest<WorkerOR>(message);
    public static WorkerOR InternalServerError(string message) => InternalServerError<WorkerOR>(message);
}