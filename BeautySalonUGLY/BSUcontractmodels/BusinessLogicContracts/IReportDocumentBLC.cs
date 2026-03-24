using BSUcontractmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

/// Сервис для формирования документов отчётов в различных форматах
public interface IReportDocumentBLC
{
    /// Сформировать Word документ с отчётом по посещениям мастера
    Stream GenerateMasterVisitsWordReport(List<MasterVisitsDM> masterVisits);

    /// Сформировать Word документ с расширенной информацией по посещения
    Stream GenerateMasterVisitsWordReportWithDates(List<MasterVisitsDM> masterVisits, DateTime dateFrom, DateTime dateTo);
}
