using BSUcontractmodels.AdapterContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BSUWebAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(IReportAdapter repAdapter) : ControllerBase
    {
        private readonly IReportAdapter _repAdapter = repAdapter;

        /// Получить отчёт по посещениям мастера за период (JSON)
        [HttpGet("master-visits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMasterVisitsReport(
            [FromQuery] string masterID,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            CancellationToken ct)
        {
            var result = await _repAdapter.GetMasterVisitsReportAsync(masterID, dateFrom, dateTo, ct);
            return result.GetResponse(Request, Response);
        }

        /// Получить отчёт по посещениям мастера в формате Word (.docx)
        [HttpGet("master-visits/word")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMasterVisitsWordReport(
            [FromQuery] string masterID,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            CancellationToken ct)
        {
            try
            {
                var (stream, fileName) = await _repAdapter.GetMasterVisitsReportWordAsync(masterID, dateFrom, dateTo, ct);

                return File(
                    stream,
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                    fileName
                );
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// Сформировать отчёт по посещениям мастера и отправить на email
        [HttpPost("master-visits/send-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMasterVisitsReportViaEmail(
            [FromQuery] string masterID,
            [FromQuery] DateTime dateFrom,
            [FromQuery] DateTime dateTo,
            [FromQuery] string toEmail,
            CancellationToken ct)
        {
            var result = await _repAdapter.GenerateAndSendReportViaEmailAsync(masterID, dateFrom, dateTo, toEmail, ct);
            return result.GetResponse(Request, Response);
        }
    }
}
