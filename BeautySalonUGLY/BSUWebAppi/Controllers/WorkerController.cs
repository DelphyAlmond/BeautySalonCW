using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BindingModels;
using BSUcontrmodels.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BSUWebAppi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class WorkerController(IWorkerAdapter adapter) : ControllerBase
    {
        private readonly IWorkerAdapter _adapter = adapter;

        [HttpGet]
        public IActionResult GetAllRecords([FromQuery] bool onlyActive = true)
        {
            return _adapter.GetList(onlyActive).GetResponse(Request, Response);
        }

        [HttpPost("{data}")]
        public IActionResult GetRecord(string data)
        {
            return _adapter.GetElement(data).GetResponse(Request, Response);
        }

        [HttpGet("by-post/{postType}")]
        public IActionResult GetByPost(Post postType, [FromQuery] bool onlyActive = true)
        {
            return _adapter.GetByPost(postType, onlyActive).GetResponse(Request, Response);
        }

        [HttpGet("by-birthdate")]
        public IActionResult GetByBirthDate([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] bool onlyActive = true)
        {
            return _adapter.GetByBirthDate(start, end, onlyActive).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult Register([FromBody] WorkerBM model)
        {
            return _adapter.RegisterWorker(model).GetResponse(Request, Response);
        }

        [HttpPost]
        public IActionResult ChangeInfo([FromBody] WorkerBM model)
        {
            return _adapter.ChangeWorkerInfo(model).GetResponse(Request, Response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return _adapter.RemoveWorker(id).GetResponse(Request, Response);
        }
    }
}