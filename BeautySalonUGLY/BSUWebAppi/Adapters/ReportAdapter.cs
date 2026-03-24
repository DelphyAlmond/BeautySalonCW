using AutoMapper;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.AdapterContracts.OperationResponses;
using BSUcontractmodels.Exceptions;
using BSUcontractmodels.OfficePackage;
using BSUcontractmodels.ViewModels;
using BSUcontractmodels.BusinessLogicContracts;
using BSUmodels.Exceptions;

namespace BSUWebAppi.Adapters;

public class ReportAdapter(IReportBLC reportBLC, IReportDocumentBLC reportDocBLC,
    ILogger<ReportAdapter> logger, IMapper mapper) : IReportAdapter
{
    private readonly IReportBLC _reportBLC = reportBLC;
    private readonly IReportDocumentBLC _reportDocBLC = reportDocBLC;
    private readonly ILogger<ReportAdapter> _logger = logger;
    private readonly IMapper _mapper = mapper;

    /// Получить отчёт по посещениям конкретного мастера с обработкой ошибок
    public async Task<ReportOR> GetMasterVisitsReportAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct)
    {
        try
        {
            // Валидация диапазона дат
            if (dateFrom > dateTo)
            {
                return ReportOR.BadRequest("< Ошибка - начальная дата не может быть позже конечной >");
            }

            // Получаем данные отчёта через BLC
            var masterVisits = await _reportBLC.GetMasterVisitsAsync(masterID, dateFrom, dateTo, ct);

            // Маппируем в ViewModel
            var vms = masterVisits.Select(x => _mapper.Map<MasterVisitsVM>(x)).ToList();

            return ReportOR.OK(vms);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "ArgumentNullException");
            return ReportOR.BadRequest("< Ошибка - данные пусты >");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOR.BadRequest($"< Ошибка валидации: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ReportOR.NotFound($"< Мастер с ID: {masterID} - не найден >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ReportOR.NotFound("< Ошибка - посещения для указанного периода не найдены >");
        }
        catch (StorageException ex)
        {
            _logger.LogError(ex, "StorageException");
            return ReportOR.InternalServerError($"< Ошибка при работе с SC: {ex.InnerException?.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOR.InternalServerError(ex.Message);
        }
    }

    /// Получить отчёт в формате Word документа
    public async Task<(Stream stream, string fileName)> GetMasterVisitsReportWordAsync(string masterID, DateTime dateFrom, DateTime dateTo, CancellationToken ct)
    {
        try
        {
            // Валидация диапазона дат
            if (dateFrom > dateTo)
                throw new ValidationException("< Ошибка - начальная дата не может быть позже конечной >");

            // Получаем данные отчёта через BLC
            var masterVisits = await _reportBLC.GetMasterVisitsAsync(masterID, dateFrom, dateTo, ct);

            if (masterVisits == null || masterVisits.Count == 0)
                throw new NullListException();

            // Формируем документ
            var stream = _reportDocBLC.GenerateMasterVisitsWordReportWithDates(masterVisits, dateFrom, dateTo);

            // Формируем имя файла
            var masterName = masterVisits.FirstOrDefault()?.MasterFullName?.Replace(" ", "_") ?? "Report";
            var fileName = $"Report_{masterName}_{dateFrom:yyyy-MM-dd}_{dateTo:yyyy-MM-dd}.docx";

            return (stream, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Word report");
            throw;
        }
    }

    /// Сформировать отчёт и отправить на email
    public async Task<ReportOR> GenerateAndSendReportViaEmailAsync(string masterID, DateTime dateFrom, DateTime dateTo, string toEmail, CancellationToken ct)
    {
        try
        {
            // Валидация входных данных
            if (dateFrom > dateTo)
                return ReportOR.BadRequest("< Ошибка - начальная дата не может быть позже конечной >");

            if (string.IsNullOrWhiteSpace(toEmail))
                return ReportOR.BadRequest("< Ошибка - email адрес не указан >");

            // Отправляем отчёт через BLC
            var result = await _reportBLC.GenerateAndSendReportAsync(masterID, dateFrom, dateTo, toEmail, ct);

             if (result)
            {
                _logger.LogInformation($"Report sent successfully to {toEmail} for master {masterID}");
                return ReportOR.NoContent();
            }

            return ReportOR.InternalServerError("Ошибка при отправке отчёта на email");
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "ValidationException");
            return ReportOR.BadRequest($"< Ошибка валидации: {ex.Message} >");
        }
        catch (ElementNotFoundException ex)
        {
            _logger.LogError(ex, "ElementNotFoundException");
            return ReportOR.NotFound($"< Мастер с ID: {masterID} - не найден >");
        }
        catch (NullListException)
        {
            _logger.LogError("NullListException");
            return ReportOR.NotFound("< Ошибка - посещения для указанного периода не найдены >");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "InvalidOperationException");
            return ReportOR.InternalServerError($"< Ошибка конфигурации: {ex.Message} >");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception");
            return ReportOR.InternalServerError($"< Ошибка при отправке отчёта: {ex.Message} >");
        }
    }
}
